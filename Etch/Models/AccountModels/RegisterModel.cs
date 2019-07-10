using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.Models.AccountModels {
    public class RegisterModel {
        // First name field
        [Required]        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        // Last name field
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Email address field
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        // Password field
        [Required]
        [StringLength(100, ErrorMessage = "The Password must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        // Confirm password field
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The Password and Confirmation Password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        // Profile photo field
        [Display(Name = "Profile Photo")]
        public byte[] ProfilePhoto { get; set; }
    }
}
