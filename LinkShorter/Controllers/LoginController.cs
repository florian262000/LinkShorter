using System;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        [Route("login")]
        /// <response code="200">login ok</response>
        /// <response code="401">invalid userdata</response>            
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Login([FromBody] LoginData loginData)
        {
            if (!CheckIfUsernameExists(loginData.Username)) return StatusCode(404, "json: user not found lul");
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

                return StatusCode(200, "json: login succeeded");
            }
            else
            {
                return StatusCode(401, "monkaTOS - invalid userdata");
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
            Console.WriteLine("session: " + _sessionManager.VerifySession(session));
            if (_sessionManager.VerifySession(session))
            {
                return StatusCode(200, "sdfsdf");
            }
            else
            {
                return StatusCode(404, "sdfsdf");
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
            if (CheckIfUsernameExists(loginData.Username))
                return StatusCode(409, "json: username already in use, try another one lul xD");

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
            return StatusCode(200, "json: yep registration successful");
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