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
        private readonly DatabaseWrapper databaseWrapper;

        public LinkApiController(DatabaseWrapper databaseWrapper)
        {
            this.databaseWrapper = databaseWrapper;
        }

        [HttpPost]
        [Route("add")]
        public LinkAddApiPost Add([FromBody] LinkAddApiPost linkAddApiPost)
        {
         
            var sql = "SELECT version()";

            using var cmd = new NpgsqlCommand(sql, databaseWrapper.GetDatabaseConnection());

            var version = cmd.ExecuteScalar().ToString();
            
            Console.WriteLine($"PostgreSQL version: {version}");
            Console.WriteLine();
            Console.WriteLine(linkAddApiPost.ToString());
            return linkAddApiPost;
        }
    }
}