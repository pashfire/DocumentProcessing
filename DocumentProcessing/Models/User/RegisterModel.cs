using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DocumentProcessing.Models.User
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(20,MinimumLength = 3, ErrorMessage = "The name should be at least 3 symbols.")]
        public string Firstname { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The last name should be at least 3 symbols.")]
        public string Lastname { get; set; }

        [Required]
        [Range(1, 5)]
        public int UserRole { get; set; }
    }
}