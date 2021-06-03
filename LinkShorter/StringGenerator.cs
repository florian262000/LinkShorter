using System;
using System.Linq;

namespace LinkShorter
{
    public class StringGenerator
    {
        public string GenerateRandomPath()
        {
            return RandomString(5).ToLower();
        }


        private static Random random = new Random();

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}