using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace LinkShorter.Controllers
{
    [ApiController]
    [Route("/{shortPath}")]
    public class ShortPathController
    {
        private readonly DatabaseWrapper _databaseWrapper;

        public ShortPathController(DatabaseWrapper databaseWrapper)
        {
            this._databaseWrapper = databaseWrapper;
        }

        [HttpGet]
        public string Get(string shortPath)
        {
            var queryTargetUrl =
                @$"SELECT targeturl FROM links WHERE shortPath = '{shortPath}'; UPDATE links set clickcounter = clickcounter+1  WHERE shortPath = '{shortPath}'; ";
            var cmdUserId = new NpgsqlCommand(queryTargetUrl, _databaseWrapper.GetDatabaseConnection());

            var targetPath = cmdUserId.ExecuteScalar()?.ToString();

            if (targetPath == null)
            {
                return "404";
            }


            return targetPath;
        }
    }
}