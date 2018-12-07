using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SD.Data.Context;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services.Contracts;
using SD.Services.External;
//using X.PagedList;

namespace SD.Services.Data.Services
{
	public class SensorDataService : ISensorDataService
	{
		private readonly IApiClient apiClient;
		private readonly DataContext dataContext;
		private readonly INotificationService notificationService;

		public SensorDataService(IApiClient aPIClient, DataContext dataContext, INotificationService notificationService)
		{
			this.apiClient = aPIClient ?? throw new ArgumentNullException(nameof(aPIClient));
			this.dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
			this.notificationService = notificationService;
		}

		//TODO: Handle exception coming from API. Catch and translate to business exception.
		//TODO: Then throw/bubble up.
		public async Task GetSensorsData()
		{
			IList<Sensor> allSensors = await this.dataContext.Sensors.ToListAsync();
			IList<SensorData> updateDataList = new List<SensorData>();
			IList<Sensor> updateSensorsList = new List<Sensor>();
			IDictionary<Sensor, SensorData> sensorsDictionary = new Dictionary<Sensor, SensorData>();

			foreach (var sensor in allSensors)
			{
				var lastTimeStamp = sensor.LastTimeStamp;
				TimeSpan difference = DateTime.Now.Subtract((DateTime)lastTimeStamp);
				var pollingInterval = sensor.MinPollingIntervalInSeconds;

				if (difference.TotalSeconds >= pollingInterval)
				{
					SensorData oldSensorData = await this.dataContext.SensorData
					.Include(sd => sd.Sensor.UserSensors)
					.Where(oSD => oSD.SensorId.Equals(sensor.SensorId))
					.OrderByDescending(oSD => oSD.TimeStamp)
					.FirstAsync();

					SensorData newSensorData = await this.apiClient
					.GetSensorData("sensorId?=" + sensor.SensorId);
					newSensorData.SensorId = sensor.SensorId;
					if (newSensorData.Value.Equals("true")) { newSensorData.Value = "1"; };
					if (newSensorData.Value.Equals("false")) { newSensorData.Value = "0"; };

					sensor.LastTimeStamp = newSensorData.TimeStamp;
					sensor.LastValue = newSensorData.Value;
					updateSensorsList.Add(sensor);

					oldSensorData.Value = newSensorData.Value;
					oldSensorData.TimeStamp = newSensorData.TimeStamp;
					updateDataList.Add(oldSensorData);

					sensorsDictionary.Add(sensor, newSensorData);
				}
			}
			var notificationsList = await this.notificationService.CheckAlarmNotifications(sensorsDictionary);
			await this.dataContext.AddRangeAsync(notificationsList);

			this.dataContext.UpdateRange(updateSensorsList);
			this.dataContext.UpdateRange(updateDataList);

			await this.dataContext.SaveChangesAsync(false);
		}



		//public async Task<SensorData> GetSensorDataByIdAsync(Guid id)
		//{
		//    return await this.dataContext.SensorData.FirstOrDefaultAsync(se => se.SensorId == id);
		//}

		public async Task<Sensor> GetSensorsByIdAsync(string id)
		{
			return await this.dataContext.Sensors
				.Include(s => s.SensorData)
				.FirstOrDefaultAsync(se => se.Id == id);
		}
	}
}
