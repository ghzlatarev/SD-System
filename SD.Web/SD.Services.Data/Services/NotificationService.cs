using Microsoft.AspNetCore.SignalR;
using SD.Services.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SD.Services.Data.Services
{
	public class NotificationService : INotificationService
	{
		private readonly IHubContext<NotificationHub> _hub;

		public NotificationService(IHubContext<NotificationHub> hub)
		{
			_hub = hub;
		}

		public Task SendNotificationAsync(string message, string userId)
		{
			return _hub.Clients.User(userId).SendAsync("ReceiveNotification", message);
		}
	}
}
