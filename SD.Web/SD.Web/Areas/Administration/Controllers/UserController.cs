using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SD.Data.Models.Identity;
using SD.Services.Data.Services.Identity.Contracts;
using SD.Web.Areas.Administration.Models.UserViewModels;
using System;
using System.Threading.Tasks;

namespace SD.Web.Areas.Admin.Controllers
{
	[Area("Administration")]
    [Authorize(Policy = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(IUserService userService, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
			this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
			this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await this.userService.FilterUsersAsync();

            var model = new UserIndexViewModel(users);

            return View(model);
        }

        [HttpGet("users/filter")]
        public async Task<IActionResult> Filter(string searchTerm, int? pageNumber, int? pageSize)
        {
            searchTerm = searchTerm ?? string.Empty;

            var users = await this.userService.FilterUsersAsync(searchTerm, pageNumber ?? 1, pageSize ?? 10);

            var model = new UserIndexViewModel(users, searchTerm);

            return PartialView("_UserTablePartial", model.Table);
        }

		[HttpGet("users/promote/{id}")]
		public async Task<IActionResult> Promote(string id)
		{
			const string adminRole = "Administrator";

			if (id == null)
			{
				throw new ApplicationException(string.Format("Id cannot be null."));
			}

			var user = await this.userManager.FindByIdAsync(id);

			if (user == null)
			{
				throw new ApplicationException(string.Format("User with id {0} was not found.", id));
			}

			var addRoleResult = await this.userManager.AddToRoleAsync(user, adminRole);
			
			if (addRoleResult.Succeeded == true)
			{
				await this.userService.UpdateRole(user);
			}
			else
			{
				throw new ApplicationException(string.Format("User promotion operation was unsuccessful."));
			}

			var model = new UserTableViewModel(user);

			return PartialView("_UserTableRowPartial", model);
		}

		[HttpGet("users/demote/{id}")]
		public async Task<IActionResult> Demote(string id)
		{
			const string adminRole = "Administrator";

			if (!await this.roleManager.RoleExistsAsync(adminRole))
			{
				throw new ApplicationException(string.Format("User demotion unsuccessful , {0} role does not exist.", adminRole));
			}

			if (id == null)
			{
				throw new ApplicationException(string.Format("Id cannot be null."));
			}
			var user = await this.userManager.FindByIdAsync(id);
			if(user == null)
			{
				throw new ApplicationException(string.Format("User with id {0} was not found.", id));
			}

			var removeRoleResult = await this.userManager.RemoveFromRoleAsync(user, adminRole);

			if (removeRoleResult.Succeeded == true)
			{
				await this.userService.UpdateRole(user);
			}
			else
			{
				throw new ApplicationException(string.Format("User demotion operation was unsuccessful."));
			}

			var model = new UserTableViewModel(user);

			return PartialView("_UserTableRowPartial", model);
		}
	}
}
