using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace wad_arba_00003741_ii.Models
{
    public class RegistrationViewModel
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        [Required]
        [MinLength(4)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        [MinLength(4)]
        public string LastName { get; set; }

        [DisplayName("Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Password")]
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Required]
        [MinLength(6)]
        public string ConfirmPassword { get; set; }
    }
}