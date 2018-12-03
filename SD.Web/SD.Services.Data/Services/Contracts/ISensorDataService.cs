using SD.Data.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SD.Services.Data.Services.Contracts
{
    public interface ISensorDataService
    {
        Task GetSensorsData();
        Task<IEnumerable<SensorData>> ListDataSensorsAsync();
        Task<IEnumerable<Sensor>> ListSensorsAsync();
        Task<SensorData> GetSensorDataByIdAsync(Guid id);
        Task<Sensor> GetSensorsByIdAsync(Guid id);
    }
}
