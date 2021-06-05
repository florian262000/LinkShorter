using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace LinkShorter.Controllers
{
    [Controller]
    [Route("/api/login")]
    public class LoginController
    {
        private DatabaseWrapper _databaseWrapper;

        public LoginController(DatabaseWrapper databaseWrapper)
        {
            this._databaseWrapper = databaseWrapper;
        }

        [Route("login")]
        [HttpPost]
        public string Login([FromBody] LoginData loginData)
        {
            Console.WriteLine("username: " + loginData.Username);
            Console.WriteLine("password: " + loginData.Password);
            return "ok";
        }
    }
}