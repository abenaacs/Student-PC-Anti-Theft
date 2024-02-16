namespace Safe_Campus.Models
{
    public class LoginResponse
    {
        public required string Token { get; set; }
        public required string Role { get; set; }
    }
}
