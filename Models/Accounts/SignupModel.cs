using System.ComponentModel.DataAnnotations;

namespace WebClient_For_Microservice.Models.Accounts
{
    public class SignupModel
    {
        [Required]
        [EmailAddress]
        [Display(Name="Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(6, ErrorMessage = "Password must be at least six characters long!")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and its confirmation does not match")]
        public string ConfirmPassword { get; set; }

    }
}
