using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.Request
{
    public class LoginRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
