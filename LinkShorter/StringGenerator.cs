using System;
using System.Linq;

namespace LinkShorter
{
    public class StringGenerator
    {
        public string GenerateRandomPath()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return RandomString(5, chars).ToLower();
        }

        public string GenerateRandomSalt()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            return RandomString(32, chars);
        }

        public string GenerateApiKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return RandomString(29, chars).ToLower();
        }


        private static Random random = new Random();

        private string RandomString(int length, string chars)
        {
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}