using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

namespace LinkShorter
{
    public class PasswordManager
    {
        private readonly StringGenerator _stringGenerator;
        private readonly ConfigWrapper _configWrapper;

        public PasswordManager(StringGenerator stringGenerator, ConfigWrapper configWrapper)
        {
            _stringGenerator = stringGenerator;
            _configWrapper = configWrapper;
        }


        public string SaltGenerator()
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            return Convert.ToBase64String(salt);
        }


        // hash with salt

        public string Hash(string password, string salt)
        {
            password += _configWrapper.Get()["password_pepper"];
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}