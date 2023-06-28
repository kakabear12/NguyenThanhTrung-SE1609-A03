using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Request
{
    public class CreateCustomerRequest
    {
        [EmailAddress(ErrorMessage = "Email must right format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "CustomerName is required field")]
        [StringLength(40, ErrorMessage = "CustomerName is least at 40 characters")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "City is required field")]
        public string City { get; set; }
        [Required(ErrorMessage = "Country is required field")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Password is required field")]
        [MinLength(6, ErrorMessage = "Password is least 6 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Birth day is required field")]
        public DateTime? Birthday { get; set; }
    }
}
