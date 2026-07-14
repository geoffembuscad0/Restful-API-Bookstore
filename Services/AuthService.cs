using System.Security.Cryptography;
using System.Text;
using RestfulApiDemo.Data;
using RestfulApiDemo.Models;

namespace RestfulApiDemo.Services
{
    public interface IAuthService
    {
        string HashPassword(string password);
        Task<User> RegisterUserAsync(string username, string password);
        Task<User> LoginUserAsync(string username, string password);
        Task LogoutUserAsync(string sessionToken);
        User GetUserBySessionToken(string sessionToken);
        string CreateSessionToken(User user);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISessionService _sessionService;

        public AuthService(ApplicationDbContext context, ISessionService sessionService)
        {
            _context = context;
            _sessionService = sessionService;
        }

        /// <summary>
        /// Hash password using MD5
        /// </summary>
        public string HashPassword(string password)
        {
            using (var md5 = MD5.Create())
            {
                var hashedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToHexString(hashedBytes);
            }
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        public async Task<User> RegisterUserAsync(string username, string password)
        {
            // Check if user already exists
            var existingUser = _context.Users
                .FirstOrDefault(u => u.Username == username && u.DeletedAt == null);

            if (existingUser != null)
            {
                throw new InvalidOperationException("Username already exists");
            }

            var user = new User
            {
                Username = username,
                Password = HashPassword(password),
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// Login user with username and password
        /// </summary>
        public async Task<User> LoginUserAsync(string username, string password)
        {
            var hashedPassword = HashPassword(password);
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username 
                    && u.Password == hashedPassword 
                    && u.DeletedAt == null);

            if (user == null)
            {
                throw new InvalidOperationException("Invalid username or password");
            }

            return user;
        }

        /// <summary>
        /// Create a session token for a user
        /// </summary>
        public string CreateSessionToken(User user)
        {
            var token = Guid.NewGuid().ToString();
            _sessionService.StoreSession(token, user.Id);
            return token;
        }

        /// <summary>
        /// Get user by session token
        /// </summary>
        public User GetUserBySessionToken(string sessionToken)
        {
            var userId = _sessionService.GetSession(sessionToken);
            if (userId == null)
            {
                throw new InvalidOperationException("Invalid session token");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId && u.DeletedAt == null);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            return user;
        }

        /// <summary>
        /// Logout user by removing session
        /// </summary>
        public async Task LogoutUserAsync(string sessionToken)
        {
            _sessionService.RemoveSession(sessionToken);
            await Task.CompletedTask;
        }
    }
}