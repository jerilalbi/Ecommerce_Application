using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ecommerce_Application.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$",ErrorMessage = "Email is not Valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [MinLength(6, ErrorMessage = "Password Must Greater than 6")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password must be Same")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone is Required")]
        [StringLength(10,MinimumLength = 10, ErrorMessage = "Phone must be 10")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "DOB is Required")]
        public DateTime Dob { get; set; }
    }
}