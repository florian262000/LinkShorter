using System.Collections.Generic;

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
            return !map.TryGetValue(sessionId, out var userid) ? null : userid;
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