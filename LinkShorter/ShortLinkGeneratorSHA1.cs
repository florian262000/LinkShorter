using System;
using System.Security.Cryptography;
using System.Text;

namespace LinkShorter.Utilities
{
    public class ShortLinkGeneratorSHA1
    {
        public int MinRouteStringLength { get; set; }
        public Func<string, bool> pathExistsFunc = null;

        public ShortLinkGeneratorSHA1(Func<string, bool> pathExistsFunc, int minRouteStringLength = 5)
        {
            this.MinRouteStringLength = minRouteStringLength;
            this.pathExistsFunc = pathExistsFunc;
        }

        public string GenerateUniqueShortPath(string userInputLink)
        {
            using SHA1 hashGen = new SHA1Managed();
            byte[] hash = hashGen.ComputeHash(Encoding.ASCII.GetBytes(userInputLink));
            StringBuilder builder = new StringBuilder();

            foreach (var b in hash)
                builder.Append(b.ToString("x2"));

            string bitString = builder.ToString();

            int startValue = 0;
            int lengthValue = this.MinRouteStringLength;
            string shortPath = bitString.Substring(startValue, lengthValue);

            while (pathExistsFunc.Invoke(shortPath))
            {
                if (++startValue == bitString.Length - this.MinRouteStringLength)
                {
                    startValue = 0;
                    lengthValue++;
                }
                else
                {
                    shortPath = bitString.Substring(startValue, lengthValue);
                }
            }

            return shortPath;
        }
    }
}