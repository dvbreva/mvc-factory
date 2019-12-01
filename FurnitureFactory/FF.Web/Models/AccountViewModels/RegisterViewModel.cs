using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FF.Web.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
      //  [Remote(action: "IsEmailInUse", controller: "Accounts")] // this attribute calls the IsEmailInUse method from the controller
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "These passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Your zipcode should match the format XXX-XXX-XX")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{2}$")]
        public string Zipcode { get; set; }
    }
}
