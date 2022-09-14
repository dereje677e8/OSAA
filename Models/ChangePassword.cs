

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;



namespace OnlineStudentAcademicAdvisory.Models
{
    public class ChangePassword
    {

        private OSAAEntities db = new OSAAEntities();

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Passowrd is required!")]
        public string oldPassword { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Passowrd is required!")]
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassowrd { get; set; }
    }
}