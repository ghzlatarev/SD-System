using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SD.Data.Models.Identity;
using SD.Services.Data.Services.Identity.Contracts;
using SD.Web.Areas.Identity.ManageViewModels;
using SD.Web.Utilities.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SD.Web.Areas.Identity.Controllers
{
    [Authorize]
    [Area("Identity")]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IUserService userService,
          ILogger<ManageController> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet("profile")]
        [ResponseCache(CacheProfileName = "Short")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new IndexViewModel(user, StatusMessage);

            return View(model);
        }

        [HttpPost("profile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError(string.Empty, string.Format("Email {0} is already taken.", model.Email));
                StatusMessage = "Error - Email already exists!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                var email = user.Email;
                if (model.Email != email)
                {
                    var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                    if (!setEmailResult.Succeeded)
                    {
                        throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                    }
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Story = model.Story;

                await _userManager.UpdateAsync(user);

                StatusMessage = "Your profile has been updated";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("change-password")]
        [ResponseCache(CacheProfileName = "Short")]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(Index));
        }

        [ValidateAntiForgeryToken]
        [HttpPost("Identity/[controller]/[action]")]
        public async Task<IActionResult> Avatar(IFormFile avatarImage)
        {
            if (avatarImage == null)
            {
                this.StatusMessage = "Error: Please provide an image!";
                return this.RedirectToAction(nameof(Index));
            }

            if (!this.IsValidImage(avatarImage))
            {
                this.StatusMessage = "Error: Image is too large or incorrect forma!";
                return this.RedirectToAction(nameof(Index));
            }

            await this._userService.SaveAvatarImageAsync(
                avatarImage.OpenReadStream(),
                this.User.GetId());

            this.StatusMessage = "Profile image successfully updated.";

            return this.RedirectToAction(nameof(Index));
        }

        [NonAction]
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [NonAction]
        private bool IsValidImage(IFormFile image)
        {
            string type = image.ContentType;
            /*Checks the format of the image/*/
            if (type != "image/png" && type != "image/jpg" && type != "image/jpeg")
            {
                return false;
            }

            /*Checks if the file is smaller than 1 MB.*/
            return image.Length < 1024 * 1024;
        }

        [NonAction]
        private string GetUploadRoot()
        {
            var environment = this.HttpContext.RequestServices
                .GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;

            return Path.Combine(environment.WebRootPath, "images", "avatars");
        }
    }
}
