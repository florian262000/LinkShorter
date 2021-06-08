using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using Npgsql;

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

        public LinkApiController(DatabaseWrapper databaseWrapper, ConfigWrapper config, StringGenerator stringGenerator,
            SessionManager sessionManager)
        {
            this._databaseWrapper = databaseWrapper;
            this._config = config;
            this._stringGenerator = stringGenerator;
            this._sessionManager = sessionManager;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromBody] LinkAddApiPost linkAddApiPost)
        {
            Request.Headers.TryGetValue("x-api-key", out var apikey);
            Request.Cookies.TryGetValue("session", out var session);


            string userId;
            if (!apikey.ToString().Equals(""))
            {
                var queryUserId = @$"SELECT id FROM users WHERE apikey = '{apikey}';";
                var cmdUserId = new NpgsqlCommand(queryUserId, _databaseWrapper.GetDatabaseConnection());

                userId = cmdUserId.ExecuteScalar()?.ToString();
            }
            else
            {
                userId = _sessionManager.GetUserFromSessionId(session);
            }
            
            if (userId == null) return Unauthorized("auth is invalid");


            if (linkAddApiPost.ShortPath != null && linkAddApiPost.ShortPath.StartsWith("api"))
                return Conflict("shortPath does not start with 'api'");
            if (!(linkAddApiPost.TargetUrl.StartsWith("http://") || linkAddApiPost.TargetUrl.StartsWith("https://")))
                return BadRequest("The target url must start with http:// or https://");
            


            // generate shortPath and set


            //todo check for duplacates
            if (linkAddApiPost.ShortPath == null)
            {
                linkAddApiPost.ShortPath = GenerateUniqueShortPath();
            }
            else
            {
                if (CheckIfDuplicateExists(linkAddApiPost.ShortPath))
                {
                    return Conflict("shortpath already in use");
                }
            }


            Console.WriteLine(linkAddApiPost.TargetUrl);
            var sql = @$"INSERT INTO links(id, targeturl, shortpath, clickcounter, createdat, creatoruuid)
            VALUES (DEFAULT,'{linkAddApiPost.TargetUrl}', '{linkAddApiPost.ShortPath.ToLower()}', 0, DEFAULT, '{userId}');";

            using var cmd = new NpgsqlCommand(sql, _databaseWrapper.GetDatabaseConnection());
            cmd.ExecuteScalar();

            var shortUrl = "" + _config.Get()["urlbase"] + "/" + linkAddApiPost.ShortPath;
            return Ok(shortUrl);
        }

        [HttpGet]
        [Route("getuniqueshortpath")]
        public string GetUniqueShortPath()
        {
            return GenerateUniqueShortPath();
        }


        private string GenerateUniqueShortPath()
        {
            while (true)
            {
                var shortPath = _stringGenerator.GenerateRandomPath();
                var duplicates = CheckIfDuplicateExists(shortPath);

                if (duplicates) continue;
                return shortPath;
            }
        }

        private bool CheckIfDuplicateExists(string shortPath)
        {
            var checkDuplicates = @$"SELECT shortpath FROM links WHERE shortpath = '{shortPath}' LIMIT 1;";
            var cmdCheckDuplicates = new NpgsqlCommand(checkDuplicates, _databaseWrapper.GetDatabaseConnection());

            var duplicates = cmdCheckDuplicates.ExecuteScalar();

            return duplicates != null;
        }
    }
}