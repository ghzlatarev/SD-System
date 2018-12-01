using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SD.Data.Context;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services.Contracts;
using SD.Services.External;

namespace SD.Services.Data.Services
{
    public class SensorDataService : ISensorDataService
    {
        private readonly IApiClient apiClient;
        private readonly DataContext dataContext;

        public SensorDataService(IApiClient aPIClient, DataContext dataContext)
        {
            this.apiClient = aPIClient ?? throw new ArgumentNullException(nameof(aPIClient));
            this.dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task GetSensorsData()
        {
            IList<Sensor> allSensors = await this.dataContext.Sensors.ToListAsync();
            IList<Guid> allSensorIds = allSensors.Select(s => s.SensorId).ToList();
            IList<SensorData> deleteList = new List<SensorData>();
            IList<SensorData> addList = new List<SensorData>();

            foreach (var id in allSensorIds)
            {
                SensorData newSensorData = await this.apiClient
                .GetSensorData("sensorId?=" + id);
                newSensorData.SensorId = id;

                IList<SensorData> oldSensorData = await this.dataContext.SensorData
                    .Where(oSD => oSD.SensorId.Equals(id))
                    .OrderBy(oSD => oSD.TimeStamp)
                    .ToListAsync();

                if (oldSensorData.Count >= 5)
                {
                    deleteList.Add(oldSensorData[0]);
                }

                addList.Add(newSensorData);
            }

            this.dataContext.SensorData.RemoveRange(deleteList);
            await this.dataContext.AddRangeAsync(addList);

            await this.dataContext.SaveChangesAsync(false);
        }

        public async Task<IEnumerable<SensorData>> ListDataSensorsAsync()
        {
            return await this.dataContext.SensorData.ToListAsync();
        }
        public async Task<IEnumerable<Sensor>> ListSensorsAsync()
        {
            return await this.dataContext.Sensors.Where(se => se.IsDeleted == false).ToListAsync();
        }

        public async Task<SensorData> GetSensorDataByIdAsync(Guid id)
        {
            return await this.dataContext.SensorData.FirstOrDefaultAsync(se => se.SensorId == id);
        }

        public async Task<Sensor> GetSensorsByIdAsync(Guid id)
        {
            return await this.dataContext.Sensors.Include(s => s.SensorData).FirstOrDefaultAsync(se => se.SensorId == id);
        }


    }
}
