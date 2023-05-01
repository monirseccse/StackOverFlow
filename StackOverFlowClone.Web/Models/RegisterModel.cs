using Autofac;
using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace StackOverFlowClone.Web.Models
{
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password constrains missmatch", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password & confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string? ReturnUrl { get; set; }
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }

        public RegisterModel()
        {

        }
        
    }
}
