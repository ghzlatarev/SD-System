using SD.Data.Models.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace SD.Services.Data.Services.Contracts
{
	public interface IUserSensorService
    {
        Task<IPagedList<UserSensor>> FilterUserSensorsAsync(string filter = "", int pageNumber = 1, int pageSize = 10);
        
        Task<IPagedList<UserSensor>> GetSensorsByUserId(string userId, int pageNumber = 1, int pageSize = 10);

        Task<UserSensor> GetSensorByIdAsync(string id);
        
        Task<UserSensor> AddUserSensorAsync(string userId, string sensorId, string name, string description,
            string latitude, string longitude, double alarmMin, double alarmMax, int pollingInterval, bool alarmTriggered, bool isPublic,
            string lastValue, string type);
        
        Task UpdateUserSensorAsync(UserSensor userSensor);

        Task<IEnumerable<UserSensor>> ListSensorsForUserAsync(string userId);
                
        Task<IEnumerable<UserSensor>> ListPublicSensorsAsync();

        Task<UserSensor> ListSensorByIdAsync(string sensorId);
    }
}
