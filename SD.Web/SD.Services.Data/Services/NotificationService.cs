using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SD.Data.Context;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Services.Data.Services
{
	public class NotificationService : INotificationService
	{
		private readonly IHubContext<NotificationHub> hub;
		private readonly DataContext dataContext;

		public NotificationService(IHubContext<NotificationHub> hub, DataContext dataContext)
		{
			this.hub = hub;
			this.dataContext = dataContext;
		}

		public Task SendNotificationAsync(string message, string userId)
		{
			return this.hub.Clients.Users(userId).SendAsync("ReceiveNotification", message);
		}

		public async Task<List<Notifications>> GetItemsAsync(string userId)
		{
			return await this.dataContext.Notifications
				.Where(n => n.UserId.Equals(userId) && n.IsRead == false)
				.ToListAsync();
		}

		public async Task CheckAlarmNotificationsAsync(IList<UserSensor> affectedSensorsList)
		{
			IList<Notifications> notificationsList = new List<Notifications>();

			foreach (var userSensor in affectedSensorsList)
			{
				var newValue = double.Parse(userSensor.Sensor.LastValue);
				bool shouldNotify = this.AccountForState(userSensor);
				if(shouldNotify == true)
				{
					string userId = userSensor.UserId;
					string message = userSensor.Name.ToUpper() + " is out of range, returning a value of "
															+ userSensor.Sensor.LastValue + "!"
															+ Environment.NewLine;

					Notifications notification = new Notifications
					{
						UserId = userId,
						Message = message
					};

					await this.SendNotificationAsync(message, userId);
					notificationsList.Add(notification);
				}
			}

			await this.SaveNotificationsAsync(notificationsList);
		}

		private bool AccountForState(UserSensor userSensor)
		{
			bool shouldNotify = false;
			var newValue = double.Parse(userSensor.Sensor.LastValue);

			if (userSensor.Sensor.IsState == true)
			{
				if (newValue == 1 && userSensor.AlarmTriggered == true)
				{
					shouldNotify = true;
				}
			}
			else
			{
				if ((newValue <= userSensor.AlarmMin || newValue >= userSensor.AlarmMax)
									&& userSensor.AlarmTriggered == true)
				{
					shouldNotify = true;
				}
			}

			return shouldNotify;
		}

		private async Task SaveNotificationsAsync(IList<Notifications> notificationsList)
		{
			await this.dataContext.AddRangeAsync(notificationsList);
			await this.dataContext.SaveChangesAsync();
		}

		public async Task ReadUnreadAsync(string userId)
		{
			var nlist = await this.dataContext.Notifications
				.Where(n => n.UserId.Equals(userId) && n.IsRead == false)
				.ToListAsync();

			foreach (var notification in nlist)
			{
				notification.IsRead = true;
			}

			this.dataContext.UpdateRange(nlist);
			await this.dataContext.SaveChangesAsync();
		}
	}
}
