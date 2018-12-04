using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SD.Data.Models.DomainModels;
using SD.Data.Models.Identity;
using SD.Services.Data.Services.Contracts;
using SD.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Web.ViewComponents
{
	public class NotificationsViewComponent : ViewComponent
	{
		private readonly INotificationService notificationService;
		private readonly UserManager<ApplicationUser> _userManager;

		public NotificationsViewComponent(INotificationService notificationService, UserManager<ApplicationUser> userManager)
		{
			this.notificationService = notificationService;
			this._userManager = userManager;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);
			var userId = user.Id;
				
			var items = await this.notificationService.GetItemsAsync(userId);
			return View(items);
		}
	}
}
