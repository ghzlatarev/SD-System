using SD.Data.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SD.Services.Data.Services.Contracts
{
    public interface ISensorService
    {
        Task RebaseSensorsAsync();
        Task<UserSensor> AddSensorAsync(Guid userId, string name, string description, int interval, string value, string coordinates, bool isPublic,
            int alarmMin, int alarmMax, DateTime createdOn, string type, DateTime timeStamp, bool alarmTriggered, Guid sensorId);
        
    }
}
