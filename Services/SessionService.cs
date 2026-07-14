namespace RestfulApiDemo.Services
{
    public interface ISessionService
    {
        void StoreSession(string token, long userId);
        long? GetSession(string token);
        void RemoveSession(string token);
        bool IsSessionValid(string token);
    }

    public class SessionService : ISessionService
    {
        private readonly Dictionary<string, SessionData> _sessions = new();
        private readonly object _lock = new();

        private class SessionData
        {
            public long UserId { get; set; }
            public DateTime CreatedAt { get; set; }
        }

        /// <summary>
        /// Store a session in memory
        /// </summary>
        public void StoreSession(string token, long userId)
        {
            lock (_lock)
            {
                _sessions[token] = new SessionData
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now
                };
            }
        }

        /// <summary>
        /// Get user ID from session token
        /// </summary>
        public long? GetSession(string token)
        {
            lock (_lock)
            {
                if (_sessions.TryGetValue(token, out var session))
                {
                    return session.UserId;
                }
            }
            return null;
        }

        /// <summary>
        /// Remove a session
        /// </summary>
        public void RemoveSession(string token)
        {
            lock (_lock)
            {
                _sessions.Remove(token);
            }
        }

        /// <summary>
        /// Check if session is valid
        /// </summary>
        public bool IsSessionValid(string token)
        {
            lock (_lock)
            {
                return _sessions.ContainsKey(token);
            }
        }
    }
}