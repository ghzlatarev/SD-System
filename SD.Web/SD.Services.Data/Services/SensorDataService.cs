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

		//TODO: Handle exception coming from API. Catch and translate to business exception.
		//TODO: Then throw/bubble up.
		public async Task GetSensorsData()
		{
			IList<Sensor> allSensors = await this.dataContext.Sensors.ToListAsync();
			IList<SensorData> deleteList = new List<SensorData>();
			IList<SensorData> addList = new List<SensorData>();
			IList<Sensor> updateList = new List<Sensor>();

			foreach (var sensor in allSensors)
			{
				var lastTimeStamp = sensor.LastTimeStamp;
				TimeSpan difference = DateTime.Now.Subtract((DateTime)lastTimeStamp);
				var pollingInterval = sensor.MinPollingIntervalInSeconds;

				if (difference.TotalSeconds > pollingInterval)
				{
					SensorData oldSensorData = await this.dataContext.SensorData
					.Where(oSD => oSD.SensorId.Equals(sensor.SensorId))
					.OrderByDescending(oSD => oSD.TimeStamp)
					.FirstAsync();

					SensorData newSensorData = await this.apiClient
					.GetSensorData("sensorId?=" + sensor.SensorId);
					newSensorData.SensorId = sensor.SensorId;

					sensor.LastTimeStamp = newSensorData.TimeStamp;
					sensor.LastValue = newSensorData.Value;
					updateList.Add(sensor);

					deleteList.Add(oldSensorData);

					addList.Add(newSensorData);
				}
			}

			this.dataContext.SensorData.RemoveRange(deleteList);
			await this.dataContext.AddRangeAsync(addList);
			this.dataContext.UpdateRange(updateList);

			await this.dataContext.SaveChangesAsync(false);
		}
	}
}
