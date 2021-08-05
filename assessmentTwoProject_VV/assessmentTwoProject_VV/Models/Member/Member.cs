using System;
using System.ComponentModel.DataAnnotations;

namespace assessmentTwoProject_VV.Models.Member
{
    public class Member
    {
        //server-side validation attributes

        [Required, StringLength(32, MinimumLength = 2, ErrorMessage = "First Name must be within the character limit")]
        public string Name { get; set; }

        [Required, StringLength(32, MinimumLength = 2, ErrorMessage = "Last Name must be within the character limit")]
        public string Surname { get; set; }

        
        [DataType(DataType.Date, ErrorMessage = "Invalid Date of Birth"), Required]
        public DateTime DOB { get; set; }

        [DataType(DataType.EmailAddress), Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber), Required]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
