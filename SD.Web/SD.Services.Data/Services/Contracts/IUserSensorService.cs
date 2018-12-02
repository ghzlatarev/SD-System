using SD.Data.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace SD.Services.Data.Services.Contracts
{
	public interface IUserSensorService
	{
		Task<IPagedList<UserSensor>> FilterUserSensorsAsync(string filter = "", int pageNumber = 1, int pageSize = 10);

		Task<IPagedList<UserSensor>> GetSensorsByUserId(Guid userId, int pageNumber = 1, int pageSize = 10);

		Task<UserSensor> AddUserSensorAsync(string userId, string sensorId, string name, string description, int latitude,
			int longitude, double alarmMin, double alarmMax, int pollingInterval, bool alarmTriggered, bool isPublic);

		Task<UserSensor> GetSensorByIdAsync(string id);

		Task UpdateUserSensorAsync(UserSensor userSensor);
	}
}
