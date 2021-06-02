using Newtonsoft.Json.Linq;
using Npgsql;

namespace LinkShorter
{
    public class DatabaseWrapper
    {
        private readonly JObject _config;

        private static NpgsqlConnection connection;

        public DatabaseWrapper(JObject config)
        {
            this._config = config;
            var cs =
                $"Host={config["database"]["host"]};Username={config["database"]["username"]};Password={config["database"]["password"]};Database={config["database"]["name"]}";

            connection = new NpgsqlConnection(cs);
            connection.Open();
        }

        public NpgsqlConnection GetDatabaseConnection()
        {
            return connection;
        }
    }
}