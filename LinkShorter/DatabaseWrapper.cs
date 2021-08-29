using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace LinkShorter
{
    public class DatabaseWrapper
    {
        private readonly JObject _config;

        private static NpgsqlConnection connection;

        private static string sqlLogin;

        public DatabaseWrapper(JObject config)
        {
            this._config = config;
            sqlLogin =
                $"Host={config["database"]["host"]};Username={config["database"]["username"]};Password={config["database"]["password"]};Database={config["database"]["name"]}";

            connection = new NpgsqlConnection(sqlLogin);
            connection.Open();
            // check (and create tables)
        }

        public void InitDatabase()
        {
            var queryAddExtension = "CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";";

            var cmd0 = new NpgsqlCommand(queryAddExtension, connection);
            cmd0.ExecuteScalar();

            var queryCheckUserTable = @"CREATE TABLE IF NOT EXISTS users (
                           id UUID  PRIMARY KEY DEFAULT uuid_generate_v4() UNIQUE,
                           username text,
                           email text,
                           password text,
                           salt text,
                           apikey text UNIQUE
                        );";
            var cmd = new NpgsqlCommand(queryCheckUserTable, connection);
            cmd.ExecuteScalar();


            var queryCheckLinkTable = @"CREATE TABLE IF NOT EXISTS links (
                           id UUID  PRIMARY KEY DEFAULT uuid_generate_v4() UNIQUE,
                           targetUrl text,
                           shortPath text UNIQUE,
                           clickCounter int,
                           createdAt timestamp  NOT NULL DEFAULT NOW(),
                           creatorUuid UUID,
                           FOREIGN KEY(creatorUuid) REFERENCES users(id)
                );
            ";
            var cmd1 = new NpgsqlCommand(queryCheckLinkTable, connection);
            cmd1.ExecuteScalar();
        }

        public bool isConnected()
        {
            return (connection.State & ConnectionState.Open) != 0;
        }

        public async Task reconnect()
        {
            connection = new NpgsqlConnection(sqlLogin);
            await connection.OpenAsync();
        }


        public NpgsqlConnection GetDatabaseConnection()
        {
            return connection;
        }
    }
}