using assessmentTwoProject_VV.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace assessmentTwoProject_VV.Models.Member
{
    public class Details
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string Name { get; set; }

        [Display(Name = "Last Name")]
        public string Surname { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
