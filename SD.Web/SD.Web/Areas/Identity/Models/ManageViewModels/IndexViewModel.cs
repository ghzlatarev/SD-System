using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SD.Data.Models.Identity;

namespace SD.Web.Areas.Identity.ManageViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {

        }

        public IndexViewModel(ApplicationUser user, string statusMessage)
        {
            this.Username = user.UserName;
            this.Email = user.Email;
            this.PhoneNumber = user.PhoneNumber;
            this.IsEmailConfirmed = user.EmailConfirmed;
            this.AvatarImage = user.AvatarImage;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Story = user.Story;
            this.GDPR = GDPR;
            this.StatusMessage = statusMessage;
        }

        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public byte[] AvatarImage { get; set; }

        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(25, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(250, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Story { get; set; }

        public string StatusMessage { get; set; }

        [Required]
        public bool GDPR { get; set; }
    }
}
