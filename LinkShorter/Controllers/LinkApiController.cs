using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace LinkShorter.Controllers
{
    [ApiController]
    [Route("api/link")]
    public class LinkApiController : ControllerBase
    {
        private readonly JObject _config;

        public LinkApiController(JObject config)
        {
            this._config = config;
        }

        [HttpPost]
        [Route("add")]
        public LinkAddApiPost Add([FromBody] LinkAddApiPost linkAddApiPost)
        {
            Console.WriteLine(linkAddApiPost.ToString());
            return linkAddApiPost;
        }
    }
}