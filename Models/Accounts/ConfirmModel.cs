using System.ComponentModel.DataAnnotations;

namespace WebClient_For_Microservice.Models.Accounts
{
    public class ConfirmModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }
    }
}
