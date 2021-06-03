using System.ComponentModel.DataAnnotations;

namespace Oz.Authentication
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
