using Microsoft.AspNetCore.Mvc;

namespace LinkShorter.Controllers
{
    [ApiController]
    [Route("/{shortPath}")]
    public class ShortPathController
    {
        [HttpGet]
        public string Get(string shortPath)
        {
            return shortPath;
        }
    }
}