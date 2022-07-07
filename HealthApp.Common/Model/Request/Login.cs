using System.ComponentModel.DataAnnotations;

namespace HealthApp.Common.Model.Request
{
    public class Login
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool AccessToken { get; set; } = false;
    }
}
