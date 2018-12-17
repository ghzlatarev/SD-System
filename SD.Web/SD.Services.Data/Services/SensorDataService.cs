using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SD.Data.Context;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services.Contracts;
using SD.Services.Data.Wrappers.Contracts;
using SD.Services.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SD.Services.Data.Services
{
	public class SensorDataService : ISensorDataService
	{
		private readonly IApiClient apiClient;
		private readonly DataContext dataContext;
		private readonly INotificationService notificationService;
		private readonly IMemoryCacheWrapper memoryCacheWrapper;

		public SensorDataService(IApiClient aPIClient, DataContext dataContext, INotificationService notificationService,
			IMemoryCacheWrapper memoryCacheWrapper)
		{
			this.apiClient = aPIClient ?? throw new ArgumentNullException(nameof(aPIClient));
			this.dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
			this.notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
			this.memoryCacheWrapper = memoryCacheWrapper ?? throw new ArgumentNullException(nameof(memoryCacheWrapper)); ;
		}

		public async Task GetSensorsDataAsync()
		{
			IEnumerable<Sensor> allSensors = await this.memoryCacheWrapper.GetOrSetCache();

			List<SensorData> updateDataList = new List<SensorData>();
			List<Sensor> updateSensorsList = new List<Sensor>();
			List<UserSensor> affectedSensorsList = new List<UserSensor>();

			foreach (var sensor in allSensors)
			{
				TimeSpan difference = DateTime.Now.Subtract((DateTime)sensor.LastTimeStamp);

				if (difference.TotalSeconds >= sensor.MinPollingIntervalInSeconds)
				{
					SensorData oldSensorData = await this.dataContext.SensorData
					.Include(sd => sd.Sensor)
					.ThenInclude(s => s.UserSensors)
					.Where(oSD => oSD.SensorId.Equals(sensor.SensorId))
					.OrderByDescending(oSD => oSD.TimeStamp)
					.FirstAsync();

					SensorData newSensorData = await this.apiClient.GetSensorData("sensorId?=" + sensor.SensorId);
					if (newSensorData != null)
					{
						newSensorData.SensorId = sensor.SensorId;
						if (newSensorData.Value.Equals("true")) { newSensorData.Value = "1"; };
						if (newSensorData.Value.Equals("false")) { newSensorData.Value = "0"; };

						oldSensorData.Sensor.LastTimeStamp = newSensorData.TimeStamp;
						oldSensorData.Sensor.LastValue = newSensorData.Value;
						updateSensorsList.Add(oldSensorData.Sensor);

						oldSensorData.Value = newSensorData.Value;
						oldSensorData.TimeStamp = newSensorData.TimeStamp;
						updateDataList.Add(oldSensorData);

						affectedSensorsList.AddRange(oldSensorData.Sensor.UserSensors);
					}
				}
			}

			await this.notificationService.CheckAlarmNotificationsAsync(affectedSensorsList);

			this.dataContext.UpdateRange(updateSensorsList);
			this.dataContext.UpdateRange(updateDataList);

			await this.dataContext.SaveChangesAsync(false);
		}
	}
}
