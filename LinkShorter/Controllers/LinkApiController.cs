using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public LinkApiController(DatabaseWrapper databaseWrapper, ConfigWrapper config, StringGenerator stringGenerator)
        {
            this._databaseWrapper = databaseWrapper;
            this._config = config;
            this._stringGenerator = stringGenerator;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromBody] LinkAddApiPost linkAddApiPost)
        {
            Request.Headers.TryGetValue("x-api-key", out var apikey);

            var queryUserId = @$"SELECT id FROM users WHERE apikey = '{apikey}';";
            var cmdUserId = new NpgsqlCommand(queryUserId, _databaseWrapper.GetDatabaseConnection());

            var userId = cmdUserId.ExecuteScalar()?.ToString();

            if (userId == null) return Unauthorized("x-api-key is invalid");
            // auth DONE

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