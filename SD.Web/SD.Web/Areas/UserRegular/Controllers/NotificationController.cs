using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SD.Data.Models.Identity;
using SD.Services.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Web.Areas.UserRegular.Controllers
{
	public class NotificationController : Controller
	{
		private readonly INotificationService notificationService;
		private readonly UserManager<ApplicationUser> userManager;

		public NotificationController(INotificationService notificationService, UserManager<ApplicationUser> userManager)
		{
			this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
			this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		}

		public async Task<IActionResult> ReadUnread()
		{
			var currentUser = await userManager.GetUserAsync(HttpContext.User);
			var userId = currentUser.Id;

			await this.notificationService.ReadUnreadAsync(userId);

			return RedirectToAction("Index", "Home", new { area = "UserRegular" });
		}
	}
}
