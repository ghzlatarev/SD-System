using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SD.Data.Context;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services.Contracts;
using SD.Services.Data.Utils;
using SD.Services.External;

namespace SD.Services.Data.Services
{
    public class SensorService : ISensorService
    {
        private readonly IApiClient apiClient;
        private readonly DataContext dataContext;

        public SensorService(IApiClient apiClient, DataContext dataContext)
        {
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            this.dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task RebaseSensorsAsync()
        {
            IEnumerable<Sensor> apiSensors = await this.apiClient.GetEntities("all");
            IList<Sensor> dbSensors = await this.dataContext.Sensors.ToListAsync();

            IList<Sensor> addList = apiSensors.Where(apiS => dbSensors.Any(dbS => dbS.SensorId.Equals(apiS.SensorId)) == false).ToList();
            
            await this.dataContext.Sensors.AddRangeAsync(addList);

            await this.dataContext.SaveChangesAsync(false);
        }

        public async Task<UserSensor> AddSensorAsync(Guid userId, string name, string description, int interval, string value, string coordinates, bool isPublic,
            int alarmMin, int alarmMax, DateTime createdOn, string type, DateTime timeStamp, bool alarmTriggered, Guid id)
        {
            var sensor = new UserSensor
            {
                UserId = userId,
                Name = name,
                Description = description,
                UserInterval = interval,
                LastValueUser = value,
                Coordinates = coordinates,
                IsPublic = isPublic,
                AlarmMin = alarmMin,
                AlarmMax = alarmMax,
                CreatedOn = createdOn,
                Type = type,
                TimeStamp = timeStamp,
                AlarmTriggered = alarmTriggered,
                SensorId = id
            };

            dataContext.UserSensors.Add(sensor);
            await dataContext.SaveChangesAsync();
            return sensor;
        }

        public async Task<IEnumerable<UserSensor>> ListSensorsForUserAsync(Guid userId)
        {
            return await this.dataContext.UserSensors.Where(se => se.IsDeleted == false && se.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<UserSensor>> ListPublicSensorsWhichDontBelongToUserAsync(Guid userId)
        {
            return await this.dataContext.UserSensors.Where(se => se.IsDeleted == false && se.UserId != userId && se.IsPublic == true).ToListAsync();
        }

        public async Task<IEnumerable<UserSensor>> ListPublicSensorsAsync()
        {
            return await this.dataContext.UserSensors.Where(se => se.IsDeleted == false && se.IsPublic == true).ToListAsync();
        }

        public async Task<UserSensor> ListSensorByIdAsync(Guid sensorId)
        {
            return await this.dataContext.UserSensors.FirstOrDefaultAsync(se => se.Id == sensorId);
        }
    }
}
