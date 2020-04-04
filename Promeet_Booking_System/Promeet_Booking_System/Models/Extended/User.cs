using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Promeet_Booking_System.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User

    {
        public string Confirm_Password { get; set; }

    }
    public class UserMetadata
    {
      

        [Required(AllowEmptyStrings =false,ErrorMessage = "Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Required")]
        [Display(Name = "Email Id")]
        [EmailAddress(ErrorMessage = "Invalid Email Id")]
        public string EmailId { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Required")]
        [Display(Name = "Contact Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Contact Number")]
        public long PhoneNo { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Required")]
        [Display(Name = "Office Address")]
        public string Office_Address { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false,ErrorMessage = "Required")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
     [Compare("Password", ErrorMessage = "Passwords Do not match")]
        public string Confirm_Password { get; set; }

    
    }
}