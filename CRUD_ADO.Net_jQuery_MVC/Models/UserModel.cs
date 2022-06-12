using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRUD_ADONet_jQuery_MVC.Models
{
    public class UserModel
    {
        public int USER_ID { get; set; }
        [Required]
        public string Fname { get; set; }
        [Required]
        public string Lname { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        public string Phone { get; set; }       
        [Required]
        public string Password { get; set; }
        [Required(ErrorMessage = "Required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public System.DateTime CreatedON { get; set; }
        public string ErrorMessage { get; set; }
    }
}