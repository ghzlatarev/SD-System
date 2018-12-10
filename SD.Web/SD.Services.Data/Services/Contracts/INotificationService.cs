using SD.Data.Models.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SD.Services.Data.Services.Contracts
{
	public interface INotificationService
	{
		Task SendNotificationAsync(string message, string userId);

		Task<List<Notifications>> GetItemsAsync(string userId);

		Task CheckAlarmNotificationsAsync(IList<UserSensor> affectedSensorsList);

		Task ReadUnreadAsync(string userId);
	}
}
