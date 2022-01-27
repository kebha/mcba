using System.ComponentModel.DataAnnotations;

namespace s3844648_a2.Models
{
    public class ChangePasswordModel
    {
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}