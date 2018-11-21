using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SD.Data.Context;
using SD.Data.Models;
using SD.Services.Data.Services.Contracts;
using SD.Services.Data.Utils;
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

            foreach (var id in allSensorIds)
            {
                SensorData newSensorData = await this.apiClient
               .GetSensorData<SensorData>("sensorId?=" + id);
                newSensorData.SensorId = id;

                IList<SensorData> oldSensorData = await this.dataContext.SensorData.ToListAsync();

                if (oldSensorData.Count >= 5)
                {
                    this.dataContext.SensorData.Remove(oldSensorData[0]);
                }

                await this.dataContext.SensorData.AddAsync(newSensorData);
            }

            await this.dataContext.SaveChangesAsync(false);
        }
    }
}
