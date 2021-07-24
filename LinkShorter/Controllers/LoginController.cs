using System;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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

        [HttpPost]
        [Route("login")]
        /// <response code="200">login ok</response>
        /// <response code="401">invalid userdata</response>            
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Login([FromBody] LoginData loginData)
        {
            var response = new JObject();
            if (!CheckIfUsernameExists(loginData.Username))
            {
                response["status"] = "error";
                response["data"] = JObject.FromObject(new {message = "user does not exits"});
                return StatusCode(404, response.ToString());
            }

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


                response["status"] = "success";
                response["data"] = JObject.FromObject(new {message = "user login was successful"});

                Response.Cookies.Append("session", _sessionManager.Register(userid));
                Console.WriteLine(response.ToString());

                return StatusCode(200, response.ToString());
            }
            else
            {
                response["status"] = "error";
                response["data"] = JObject.FromObject(new {message = "password does not match"});
                return StatusCode(401, response.ToString());
            }
        }

        [HttpPost]
        [Route("validatesession/{session}")]
        /// <response code="200">session ok</response>
        /// <response code="404">session not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult ValidateSession(string session)
        {
            var response = new JObject();
            Console.WriteLine("session: " + _sessionManager.VerifySession(session));
            if (session != null && _sessionManager.VerifySession(session))
            {
                response["status"] = "success";
                response["data"] = JObject.FromObject(new {message = "session still alive"});

                return StatusCode(200, response.ToString());
            }
            else
            {
                response["status"] = "error";
                response["data"] = JObject.FromObject(new {message = "session is invalid"});
                return StatusCode(404, response.ToString());
            }
        }

        [Route("getusername")]
        [HttpPost]
        /// <response code="401">conflict </response>
        /// <response code="200">reg successfull</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult GetUserName()
        {
            var response = new JObject();

            Request.Cookies.TryGetValue("session", out var sessionId);
            if (sessionId != null && _sessionManager.VerifySession(sessionId))
            {
                //todo change to correct name
                response["status"] = "success";
                response["data"] = JObject.FromObject(new {name = "Markus", message = "username"});
                return StatusCode(200, response.ToString());
            }
            else
            {
                response["status"] = "error";
                response["data"] = JObject.FromObject(new {message = "user is not lodged in"});
                return StatusCode(401, response.ToString());
            }
        }

        [Route("register")]
        [HttpPost]
        /// <response code="409">conflict </response>
        /// <response code="200">reg successfull</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Register([FromBody] LoginData loginData)
        {
            var response = new JObject();

            if (CheckIfUsernameExists(loginData.Username))
            {
                response["status"] = "error";
                response["data"] = JObject.FromObject(new {message = "username already in use, try another one"});
                return StatusCode(409, response.ToString());
            }

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

            response["status"] = "success";
            response["data"] = JObject.FromObject(new {message = "login successful"});
            return StatusCode(200, response.ToString());
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