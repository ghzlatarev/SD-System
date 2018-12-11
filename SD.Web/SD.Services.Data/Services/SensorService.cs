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
            IEnumerable<Sensor> apiSensors = await this.apiClient.GetApiSensors();
            IList<Sensor> dbSensors = await this.dataContext.Sensors.ToListAsync();

            IList<Sensor> addList = apiSensors.Where(apiS => dbSensors.Any(dbS => dbS.SensorId.Equals(apiS.SensorId)) == false).ToList();

            await this.dataContext.Sensors.AddRangeAsync(addList);

            await this.dataContext.SaveChangesAsync(false);
        }

        public async Task<IEnumerable<Sensor>> ListSensorsAsync()
        {
            return await this.dataContext.Sensors.Where(se => se.IsDeleted == false).ToListAsync();
        }

		public async Task<IEnumerable<Sensor>> ListStateSensorsAsync()
		{
			return await this.dataContext.Sensors
				.Where(se => se.IsDeleted == false && se.IsState == true)
				.ToListAsync();
		}

		public async Task<IEnumerable<Sensor>> ListNonStateSensorsAsync()
		{
			return await this.dataContext.Sensors
				.Where(se => se.IsDeleted == false && se.IsState == false)
				.ToListAsync();
		}

		public async Task<Sensor> GetSensorByIdAsync(string sensorId)
		{
			return await this.dataContext.Sensors
                .Include(se => se.SensorData)
				.FirstOrDefaultAsync(s => s.Id.Equals(sensorId));
		}
    }
}
