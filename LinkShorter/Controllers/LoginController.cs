using System;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace LinkShorter.Controllers
{
    [Controller]
    [Route("/api/login")]
    public class LoginController : ControllerBase
    {
        private DatabaseWrapper _databaseWrapper;
        private PasswordManager _passwordManager;
        private readonly StringGenerator _stringGenerator;
        private readonly SessionManager _sessionManager;

        public LoginController(DatabaseWrapper databaseWrapper, PasswordManager passwordManager,
            StringGenerator stringGenerator, SessionManager sessionManager)
        {
            this._databaseWrapper = databaseWrapper;
            this._passwordManager = passwordManager;
            this._stringGenerator = stringGenerator;
            this._sessionManager = sessionManager;
        }


        [Route("login")]
        [HttpPost]
        public string Login([FromBody] LoginData loginData)
        {
            if (!CheckIfUsernameExists(loginData.Username)) return "username is invalid";
            // validate password
            // get salt
            var sqlQuerySalt = @$"SELECT salt, password, id FROM users WHERE username = '{loginData.Username}';";
            var querySalt = new NpgsqlCommand(sqlQuerySalt, _databaseWrapper.GetDatabaseConnection());
            var result = querySalt.ExecuteReader();

            result.Read();

            var salt = result.GetString(0);
            var password = result.GetString(1);
            var userid = result.GetGuid(2).ToString();
            // hash
            result.Close();
            Console.WriteLine("uuid:" + userid);

            var hashedUserPasswordInput = _passwordManager.Hash(loginData.Password, salt);

            // set cookie

            if (hashedUserPasswordInput.Equals(password))
            {
                // set cookies
                Response.Cookies.Append("session", _sessionManager.Register(userid));

                return "ok";
            }
            else
            {
                return "monkaTOS - invalid userdata";
            }
        }

        [HttpPost]
        [Route("validatesession/{session}")]
        public bool ValidateSession(string session)
        {
            Console.WriteLine("session: " + _sessionManager.VerifySession(session));
            return _sessionManager.VerifySession(session);
        }

        /*[HttpPost]
        [Route("getusername/{session}")]
        public string GetUsername(string session)
        {
            var sw = new Stopwatch();
            sw.Start();

            while (true)
            {
                if (sw.ElapsedMilliseconds <= 5000) continue;
                sw.Stop();
                break;
            }

            var userid = _sessionManager.GetUserFromSessionId(session);

            var usernameSql = @$"SELECT username FROM users WHERE id = '{userid}' LIMIT 1;";
            var usernameQuery = new NpgsqlCommand(usernameSql, _databaseWrapper.GetDatabaseConnection());

            var result = usernameQuery.ExecuteReader();
            result.Read();
            var username = "" + result.GetString(0);
            result.Close();


            return username;
        }*/

        [Route("register")]
        [HttpPost]
        public string Register([FromBody] LoginData loginData)
        {
            if (CheckIfUsernameExists(loginData.Username)) return "username is already in use";

            var salt = _passwordManager.SaltGenerator();

            var hash = _passwordManager.Hash(loginData.Password, salt);


            string apikey;
            while (true)
            {
                apikey = _stringGenerator.GenerateApiKey();
                if (!CheckIfDuplicateApikeyExists(apikey)) break;
            }


            var insert =
                @$"INSERT INTO users(id, username, password, salt, apikey) VALUES (DEFAULT,'{loginData.Username}', '{hash}', '{salt}', '{apikey}');
                SELECT id FROM users WHERE username = '{loginData.Username}';";
            var insertion = new NpgsqlCommand(insert, _databaseWrapper.GetDatabaseConnection());
            var result = insertion.ExecuteScalar();


            var resp = new HttpResponseMessage();

            Console.WriteLine("userid: " + result.ToString());

            Response.Cookies.Append("session", _sessionManager.Register(result.ToString()));
            return "ok";
        }


        private bool CheckIfUsernameExists(string username)
        {
            var checkDuplicates = @$"SELECT username FROM users WHERE username = '{username}' LIMIT 1;";
            var cmdCheckDuplicates = new NpgsqlCommand(checkDuplicates, _databaseWrapper.GetDatabaseConnection());

            var duplicates = cmdCheckDuplicates.ExecuteScalar();

            return duplicates != null;
        }

        private bool CheckIfDuplicateApikeyExists(string apikey)
        {
            var checkDuplicates = @$"SELECT apikey FROM users WHERE apikey = '{apikey}' LIMIT 1;";
            var cmdCheckDuplicates = new NpgsqlCommand(checkDuplicates, _databaseWrapper.GetDatabaseConnection());

            var duplicates = cmdCheckDuplicates.ExecuteScalar();

            return duplicates != null;
        }
    }
}