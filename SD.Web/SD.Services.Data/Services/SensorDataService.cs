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
using X.PagedList;

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
			IList<SensorData> deleteList = new List<SensorData>();
			IList<SensorData> addList = new List<SensorData>();
			IList<Sensor> updateList = new List<Sensor>();

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

					await CheckForAlarm(sensor, newSensorData);

					addList.Add(newSensorData);
					deleteList.Add(oldSensorData);
					updateList.Add(sensor);

				}
			}

			await this.dataContext.AddRangeAsync(addList);
			this.dataContext.SensorData.RemoveRange(deleteList);
			this.dataContext.UpdateRange(updateList);

            await this.dataContext.SaveChangesAsync(false);
        }

        

        //public async Task<SensorData> GetSensorDataByIdAsync(Guid id)
        //{
        //    return await this.dataContext.SensorData.FirstOrDefaultAsync(se => se.SensorId == id);
        //}

        public async Task<Sensor> GetSensorsByIdAsync(string id)
        {
            return await this.dataContext.Sensors.Include(s => s.SensorData).FirstOrDefaultAsync(se => se.SensorId == id);
        }


		private async Task CheckForAlarm(Sensor sensor, SensorData newSensorData)
		{
			var newValue = double.Parse(newSensorData.Value);
			//This ToListAsync is using X.PagedList
			var currentUserSensors = await sensor.UserSensors.ToListAsync();
			foreach (var userSensor in currentUserSensors)
			{
				if ((newValue <= userSensor.AlarmMin || newValue >= userSensor.AlarmMax) && userSensor.AlarmTriggered == true)
				{
					var userId = userSensor.UserId.ToString();
					var message = userSensor.Name + " pinged, something is happening!";
					await this.notificationService.SendNotificationAsync(message, userId);
				}
			}
		}
	}
}
