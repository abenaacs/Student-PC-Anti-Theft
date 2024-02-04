using System.Security.Cryptography;

namespace Safe_Campus.Models
{
    public class UserDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
