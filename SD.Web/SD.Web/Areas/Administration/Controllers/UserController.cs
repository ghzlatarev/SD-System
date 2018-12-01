using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SD.Data.Models.Identity;
using SD.Services.Data.Services.Identity.Contracts;
using SD.Web.Areas.Administration.Models.UserViewModels;

namespace SD.Web.Areas.Admin.Controllers
{
    [Area("Administration")]
    [Authorize(Policy = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserService userService, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.FilterUsersAsync();

            var model = new UserIndexViewModel(users);

            return View(model);
        }

        [HttpGet("users/filter")]
        public async Task<IActionResult> Filter(string searchTerm, int? pageNumber, int? pageSize)
        {
            searchTerm = searchTerm ?? string.Empty;

            var users = await _userService.FilterUsersAsync(searchTerm, pageNumber ?? 1, pageSize ?? 10);

            var model = new UserIndexViewModel(users, searchTerm);

            return PartialView("_UserTablePartial", model.Table);
        }

		[HttpGet("users/promote/{id}")]
		public async Task<IActionResult> Promote(string id)
		{
			const string adminRole = "Administrator";

			var user = await _userManager.FindByIdAsync(id);
			var addRoleResult = await _userManager.AddToRoleAsync(user, adminRole);
			
			if (addRoleResult.Succeeded == true)
			{
				await this._userService.UpdateRole(user);
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

			if (!await _roleManager.RoleExistsAsync(adminRole))
			{
				throw new ApplicationException(string.Format("User demotion unsuccessful , {0} role does not exists.", adminRole));
			}

			var user = await _userManager.FindByIdAsync(id);
			var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, adminRole);

			if (removeRoleResult.Succeeded == true)
			{
				await this._userService.UpdateRole(user);
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
