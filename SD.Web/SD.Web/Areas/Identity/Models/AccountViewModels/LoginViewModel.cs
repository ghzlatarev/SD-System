using System.ComponentModel.DataAnnotations;

namespace SD.Web.Areas.Identity.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string UserNameOrEmail { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
