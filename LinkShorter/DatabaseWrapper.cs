using System;
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
            Console.WriteLine();
            var cs =
                $"Host={config["database"]["host"]};Username={config["database"]["username"]};Password={config["database"]["password"]};Database={config["database"]["name"]}";

            connection = new NpgsqlConnection(cs);
            connection.Open();
            // check (and create tables)

            var queryCheckUserTable = @"CREATE TABLE IF NOT EXISTS users (
                           id UUID  PRIMARY KEY,
                           username text,
                           email text
                        );";
            var cmd = new NpgsqlCommand(queryCheckUserTable, connection);
            cmd.ExecuteScalar();


            var queryCheckLinkTable = @"CREATE TABLE IF NOT EXISTS links (
                           id UUID  PRIMARY KEY,
                           targetUrl text,
                           shortPath text,
                           clickCounter int,
                           createdAt timestamp,
                           creatorUuid UUID,
                           FOREIGN KEY(creatorUuid) REFERENCES users(id)
                );
            ";
            var cmd1 = new NpgsqlCommand(queryCheckLinkTable, connection);
            cmd1.ExecuteScalar().ToString();
        }

        public NpgsqlConnection GetDatabaseConnection()
        {
            return connection;
        }
    }
}