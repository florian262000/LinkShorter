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

        public LinkApiController(DatabaseWrapper databaseWrapper)
        {
            this._databaseWrapper = databaseWrapper;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromBody] LinkAddApiPost linkAddApiPost)
        {
            Request.Headers.TryGetValue("x-api-key", out var apikey);

            var queryUserId = @$"SELECT id FROM users WHERE apikey = '{apikey}';";
            var cmdUserId = new NpgsqlCommand(queryUserId, _databaseWrapper.GetDatabaseConnection());

            var userId = cmdUserId.ExecuteScalar()?.ToString();

            if (userId == null) return Unauthorized("x-api-key ist invalid");
            if (linkAddApiPost.shortPath.StartsWith("api")) return Conflict("shortPath does not start with 'api'");
            if (!(linkAddApiPost.targetUrl.StartsWith("http://") || linkAddApiPost.targetUrl.StartsWith("https://")))
                return BadRequest("The target url must start with http:// or https://");

            Console.WriteLine(linkAddApiPost.targetUrl);
            var sql = @$"INSERT INTO links(id, targeturl, shortpath, clickcounter, createdat, creatoruuid)
            VALUES (DEFAULT,'{linkAddApiPost.targetUrl}', '{linkAddApiPost.shortPath}', 0, DEFAULT, '{userId}');";

            using var cmd = new NpgsqlCommand(sql, _databaseWrapper.GetDatabaseConnection());
            cmd.ExecuteScalar();

            return Ok();
        }
    }
}