using System;
using System.Collections.Generic;
using System.Globalization;

namespace LinkShorter
{
    public class SessionManager
    {
        private readonly StringGenerator _stringGenerator;
        private readonly Dictionary<string, string> map = new();

        public SessionManager(StringGenerator stringGenerator)
        {
            _stringGenerator = stringGenerator;
        }

        public string Register(string userId)
        {
            var sessionId = GenerateSessionId();
            map.Add(sessionId, userId);
            return sessionId;
        }

        public string GetUserFromSessionId(string sessionId)
        {

            map.TryGetValue(sessionId, out var userid);
            return userid;
        }

        public bool VerifySession(string sessionId)
        {
            return GetUserFromSessionId(sessionId) != null;
        }


        private string GenerateSessionId()
        {
            while (true)
            {
                var sessionId = _stringGenerator.GenerateSessionId();
                if (!map.ContainsKey(sessionId)) return sessionId;
            }
        }
    }
}