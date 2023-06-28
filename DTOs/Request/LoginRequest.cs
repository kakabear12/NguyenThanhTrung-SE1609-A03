using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is a required field")]
        [EmailAddress(ErrorMessage = "Email must right format form")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Password is a required field")]
        public string Password { get; set; }
    }
}
