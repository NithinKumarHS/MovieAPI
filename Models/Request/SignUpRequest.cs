using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.Request
{
    public class SignUpRequest
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
