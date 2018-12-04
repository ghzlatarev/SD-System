using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SD.Data.Context;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Services.Data.Services
{
	public class NotificationService : INotificationService
	{
		private readonly IHubContext<NotificationHub> _hub;
		private readonly DataContext dataContext;

		public NotificationService(IHubContext<NotificationHub> hub, DataContext dataContext)
		{
			_hub = hub;
			this.dataContext = dataContext;
		}

		public Task SendNotificationAsync(string message, string userId)
		{
			return _hub.Clients.Users(userId).SendAsync("ReceiveNotification", message);
		}

		public async Task<List<Notification>> GetItemsAsync(string userId)
		{
			return await this.dataContext.Notifications.Where(n => n.UserId.ToString().Equals(userId)).ToListAsync();
		}

		public async Task<IList<Notification>> CheckAlarmNotifications(IDictionary<Sensor, SensorData> sensorsDictionary)
		{
			IList<Notification> notificationsList = new List<Notification>();

			foreach (var kvp in sensorsDictionary)
			{
				var newValue = double.Parse(kvp.Value.Value);

				var currentUserSensors = kvp.Key.UserSensors;
				foreach (var userSensor in currentUserSensors)
				{
					if ((newValue <= userSensor.AlarmMin || newValue >= userSensor.AlarmMax) && userSensor.AlarmTriggered == true)
					{
						var userId = userSensor.UserId.ToString();
						var message = userSensor.Name + " pinged, something is happening!";
						await this.SendNotificationAsync(message, userId);

						Notification notification = new Notification
						{
							UserId = Guid.Parse(userId),
							Message = message
						};

						notificationsList.Add(notification);
					}
				}
			}

			return notificationsList;
		}
	}
}
