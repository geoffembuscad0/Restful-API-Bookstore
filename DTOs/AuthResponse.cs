namespace RestfulApiDemo.DTOs
{
    public class AuthResponse
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}