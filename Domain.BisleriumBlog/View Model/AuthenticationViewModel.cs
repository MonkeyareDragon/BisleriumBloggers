using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBlog.View_Model
{
    public class AuthenticationViewModel
    {
        public class ChangePasswordViewModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            public required string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm New Password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public required string ConfirmPassword { get; set; }
        }
        public class LoginViewModel
        {
            [EmailAddress]
            public string? Email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }
            public string? Username {  get; set; }
        }
        public class RegisterViewModel
        {
            [Required]
            [EmailAddress]
            public string? Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string? ConfirmPassword { get; set; }
            [Required]
            public string? Username { get; set; }

            [Required]
            public string? Role { get; set; } // Property to specify the role of the user

            public string? Address { get; set; }

            public string? Gender { get; set; }
        }
        public class UpdateUserViewModel
        {
            [Required]
            [EmailAddress]
            public string? Email { get; set; }
            [Required]
            public string? Role { get; set; }
        }
    }
}
