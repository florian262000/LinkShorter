using System;
using System.Collections.Generic;
using System.Globalization;

namespace LinkShorter
{
    public class SessionManager
    {
        private readonly StringGenerator _stringGenerator;
        private readonly Dictionary<string, string> _map = new Dictionary<string, string>();

        public SessionManager(StringGenerator stringGenerator)
        {
            _stringGenerator = stringGenerator;
        }

        public string Register(string userId)
        {
            var sessionId = GenerateSessionId();
            _map.Add(sessionId, userId);
            return sessionId;
        }

        public string GetUserFromSessionId(string sessionId)
        {
            _map.TryGetValue(sessionId, out var userid);
            return userid;
        }

        public bool VerifySession(string sessionId)
        {
            return GetUserFromSessionId(sessionId) != null;
        }

        public bool RemoveSession(string sessionId)
        {
            return _map.Remove(sessionId);
        }


        private string GenerateSessionId()
        {
            while (true)
            {
                var sessionId = _stringGenerator.GenerateSessionId();
                if (!_map.ContainsKey(sessionId)) return sessionId;
            }
        }
    }
}