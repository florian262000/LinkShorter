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
        private readonly JObject _config;
        private readonly NpgsqlConnection _npgsqlConnection;

        public LinkApiController(JObject config, NpgsqlConnection sqlConnection)
        {
            this._config = config;
            this._npgsqlConnection = sqlConnection;
        }

        [HttpPost]
        [Route("add")]
        public LinkAddApiPost Add([FromBody] LinkAddApiPost linkAddApiPost)
        {
            _npgsqlConnection.Open();
            var sql = "SELECT version()";

            using var cmd = new NpgsqlCommand(sql, _npgsqlConnection);

            var version = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"PostgreSQL version: {version}");
            Console.WriteLine();
            Console.WriteLine(linkAddApiPost.ToString());
            return linkAddApiPost;
        }
    }
}