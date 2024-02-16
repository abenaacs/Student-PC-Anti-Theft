namespace Safe_Campus.Models
{
    public class RegisterDto
    {
        public required string UserName { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string ProfilePicture { get; set; }
        public required string ContactNumber { get; set; }
        public required string Sex { get; set; }
    }
}
