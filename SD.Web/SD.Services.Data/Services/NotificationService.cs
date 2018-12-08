using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SD.Data.Context;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services.Contracts;
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

		public async Task<IList<Notifications>> CheckAlarmNotifications(IDictionary<Sensor, SensorData> sensorsDictionary)
		{
			IList<Notifications> notificationsList = new List<Notifications>();

			foreach (var kvp in sensorsDictionary)
			{
				var newValue = double.Parse(kvp.Value.Value);

				var currentUserSensors = kvp.Key.UserSensors;
				foreach (var userSensor in currentUserSensors)
				{
					if ((newValue <= userSensor.AlarmMin || newValue >= userSensor.AlarmMax) && userSensor.AlarmTriggered == true)
					{
						var userId = userSensor.UserId.ToString();
						var message = userSensor.Name.ToUpper() + " is out of range, returning a value of "
																+ userSensor.Sensor.LastValue
																+ "\n"
																+ "---------------------------";
						await this.SendNotificationAsync(message, userId);

						Notifications notification = new Notifications
						{
							UserId = userId,
							Message = message
						};

						notificationsList.Add(notification);
					}
				}
			}

			return notificationsList;
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
