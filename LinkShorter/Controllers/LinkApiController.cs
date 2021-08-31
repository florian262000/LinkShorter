using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinkShorter.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using Npgsql.TypeHandlers;
using NpgsqlTypes;

namespace LinkShorter.Controllers
{
    [ApiController]
    [Route("api/link")]
    public class LinkApiController : ControllerBase
    {
        private readonly DatabaseWrapper _databaseWrapper;
        private readonly ConfigWrapper _config;
        private readonly StringGenerator _stringGenerator;
        private readonly SessionManager _sessionManager;
        private readonly ShortLinkGeneratorSHA1 generator;

        public LinkApiController(ConfigWrapper config, StringGenerator stringGenerator,
            SessionManager sessionManager)
        {
            this._databaseWrapper = new DatabaseWrapper(config.Get());
            generator = new ShortLinkGeneratorSHA1(this.CheckIfShortPathIsAlreadyInUse);

            this._config = config;
            this._stringGenerator = stringGenerator;
            this._sessionManager = sessionManager;
        }


        [HttpPost]
        [Route("add")]
        /// <summary>
        ///     response model 
        ///     {
        ///        "shortpath": "SHORT_PATH"
        ///     }
        ///
        /// </summary>
        /// <response code="200">login ok</response>
        /// <response code="401">invalid userdata</response>
        /// <response code="400">shortpath does not match the requirements</response> 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Add([FromBody] LinkAddApiPost linkAddApiPost)
        {
            if (!_databaseWrapper.isConnected()) _databaseWrapper.reconnect();


            Request.Headers.TryGetValue("x-api-key", out var apikey);
            Request.Cookies.TryGetValue("session", out var session);

            var response = new JObject();

            string userId;
            if (!apikey.ToString().Equals(""))
            {
                var queryUserId = @$"SELECT id FROM users WHERE apikey = @apikey;";
                var cmdUserId = new NpgsqlCommand(queryUserId, _databaseWrapper.GetDatabaseConnection());
                cmdUserId.Parameters.AddWithValue("apikey", apikey);

                userId = cmdUserId.ExecuteScalar()?.ToString();
                cmdUserId.Connection?.Close();
            }
            else
            {
                userId = _sessionManager.GetUserFromSessionId(session);
            }


            if (userId == null)
            {
                response["errorMessage"] = "auth is invalid";
                return StatusCode(401, response.ToString());
            }


            if (linkAddApiPost.ShortPath != null
                && linkAddApiPost.ShortPath.StartsWith("api")
                && linkAddApiPost.ShortPath.Length >= 4
                && linkAddApiPost.ShortPath.Length <= 64)
            {
                response["errorMessage"] = "shortPath does not start with 'api'";
                return StatusCode(409, response.ToString());
            }

            if (!(linkAddApiPost.TargetUrl.StartsWith("http://") || linkAddApiPost.TargetUrl.StartsWith("https://")))
            {
                response["errorMessage"] =
                    "The target url must start with http:// or https:// and the commited shortlink need to be at leat 4 and up to 64 characters";
                return StatusCode(400, response.ToString());
            }

            // generate shortPath and set


            //todo check for duplicates
            if (linkAddApiPost.ShortPath == null)
            {
                linkAddApiPost.ShortPath = GenerateUniqueShortPath(linkAddApiPost.TargetUrl);
            }
            else
            {
                if (CheckIfShortPathIsAlreadyInUse(linkAddApiPost.ShortPath))
                {
                    response["errorMessage"] = "shortpath already in use";
                    return StatusCode(409, response.ToString());
                }
            }

            Console.WriteLine(linkAddApiPost.TargetUrl);
            var sql = @$"INSERT INTO links(id, targeturl, shortpath, clickcounter, createdat, creatoruuid)
            VALUES (DEFAULT, @TargetUrl, @ShortPath, 0, DEFAULT, @userId);";
            using var cmd = new NpgsqlCommand(sql, _databaseWrapper.GetDatabaseConnection());

            var userIdCast = Guid.Parse(userId);

            cmd.Parameters.AddWithValue("TargetUrl", linkAddApiPost.TargetUrl);
            cmd.Parameters.AddWithValue("ShortPath", linkAddApiPost.ShortPath.ToLower());
            cmd.Parameters.AddWithValue("userId", userIdCast);

            cmd.Prepare();

            cmd.ExecuteScalar();

            var shortUrl = "" + _config.Get()["urlbase"] + "/" + linkAddApiPost.ShortPath;
            response["shortpath"] = shortUrl;

            return StatusCode(200, response.ToString());
        }

        [HttpDelete]
        [Route("add")]
        /// <summary>
        ///     response model 
        ///     {
        ///        "shortpath": "SHORT_PATH"
        ///     }
        ///
        /// </summary>
        /// <response code="200">removed successfully</response>
        /// <response code="401">invalid auth</response>            
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Remove([FromBody] LinkApiRemove linkApiRemove)
        {
            if (!_databaseWrapper.isConnected()) _databaseWrapper.reconnect();


            Request.Headers.TryGetValue("x-api-key", out var apikey);
            Request.Cookies.TryGetValue("session", out var session);

            var response = new JObject();

            string userId;
            if (!apikey.ToString().Equals(""))
            {
                var queryUserId = @$"SELECT id FROM users WHERE apikey = @apikey;";
                var cmdUserId = new NpgsqlCommand(queryUserId, _databaseWrapper.GetDatabaseConnection());

                cmdUserId.Parameters.AddWithValue("apikey", apikey);
                cmdUserId.Prepare();

                userId = cmdUserId.ExecuteScalar()?.ToString();
                cmdUserId.Connection?.Close();
            }
            else
            {
                userId = _sessionManager.GetUserFromSessionId(session);
            }


            if (userId == null)
            {
                response["errorMessage"] = "auth is invalid";
                return StatusCode(401, response.ToString());
            }

            var sqlQuery =
                @$"DELETE FROM links WHERE creatoruuid = @userId AND shortpath = @ShortPath;";
            var query = new NpgsqlCommand(sqlQuery, _databaseWrapper.GetDatabaseConnection());
            var userIdCast = Guid.Parse(userId);
            query.Parameters.AddWithValue("userId", userIdCast);
            query.Parameters.AddWithValue("shortpath", linkApiRemove.ShortPath);
            query.Prepare();

            query.ExecuteNonQuery();

            return StatusCode(200, response.ToString());
        }

        [HttpGet]
        [Route("getuniqueshortpath")]
        /// <summary>
        ///    requst model
        ///    {
        ///        "TargetUrl": "TARGET_URL"
        ///    }
        /// 
        ///     response model 
        ///     {
        ///        "randomShortPath": "SHORT_PATH"
        ///     }
        ///
        /// </summary>
        /// <response code="200">short path</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUniqueShortPath([FromBody] ShortPathGenerate shortPathGenerate)
        {
            var response = new JObject();
            response["randomShortPath"] = GenerateUniqueShortPath(shortPathGenerate.TargetUrl);

            return StatusCode(200, response.ToString());
        }


        [HttpGet]
        [Route("getuserspecificlinks")]
        /// <summary>
        /// Sample request:
        ///
        ///     response model 
        ///     {
        ///         [
        ///                 {
        ///                     "Id": "",
        ///                     "TargetUrl": "",
        ///                     "ShortPath": "",
        ///                     "FullShortUrl": "",
        ///                     "ClickCounter": "",
        ///                     "TimeStamp": "",
        ///                     "CreatorId": ""
        ///                 }       
        ///         ]
        ///     }
        ///
        /// </summary>
        /// <response code="200">login ok</response>
        /// <response code="401">unautherized</response>            
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetUserSpecificLinks()
        {
            if (!_databaseWrapper.isConnected()) _databaseWrapper.reconnect();


            var response = new JObject();

            Request.Cookies.TryGetValue("session", out var session);
            if (session == null)
            {
                response["errorMessage"] = "Unauthorized";
                return StatusCode(401, response.ToString());
            }

            var userId = _sessionManager.GetUserFromSessionId(session);
            if (userId == null)
            {
                response["errorMessage"] = "Unauthorized";
                return StatusCode(401, response.ToString());
            }


            var links = new List<LinkData>();

            var sqlQuery =
                @$"SELECT id, targeturl, shortpath, clickcounter, createdat, creatoruuid FROM links WHERE creatoruuid = @userId;";
            var query = new NpgsqlCommand(sqlQuery, _databaseWrapper.GetDatabaseConnection());
            var userIdCast = Guid.Parse(userId);
            query.Parameters.AddWithValue("userId", userIdCast);
            query.Prepare();
            var result = query.ExecuteReader();

            while (result.Read())
            {
                var linkData = new LinkData()
                {
                    Id = result.GetGuid(0),
                    TargetUrl = result.GetString(1),
                    ShortPath = result.GetString(2),
                    FullShortUrl = _config.Get()["urlbase"] + "/" + result.GetString(2),
                    ClickCounter = result.GetInt32(3),
                    TimeStamp = result.GetTimeStamp(4).ToString(),
                    CreatorId = result.GetGuid(5)
                };

                links.Add(linkData);
            }

            result.Close();

            var jsonArray = JsonConvert.SerializeObject(links);
            return StatusCode(200, jsonArray);
        }


        private string GenerateUniqueShortPath(string targetUrl)
        {
            return generator.GenerateUniqueShortPath(targetUrl);
        }


        private bool CheckIfShortPathIsAlreadyInUse(string shortPath)
        {
            if (!_databaseWrapper.isConnected()) _databaseWrapper.reconnect();

            var checkDuplicates = @$"SELECT shortpath FROM links WHERE shortpath = @shortPath LIMIT 1;";
            var cmdCheckDuplicates = new NpgsqlCommand(checkDuplicates, _databaseWrapper.GetDatabaseConnection());
            cmdCheckDuplicates.Parameters.AddWithValue("shortPath", shortPath);
            cmdCheckDuplicates.Prepare();

            var duplicates = cmdCheckDuplicates.ExecuteScalar();

            return duplicates != null;
        }
    }
}