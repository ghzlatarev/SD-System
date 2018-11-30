using Microsoft.EntityFrameworkCore;
using SD.Data.Context;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Exceptions;
using SD.Services.Data.Services.Contracts;
using SD.Services.Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace SD.Services.Data.Services
{
	public class UserSensorService : IUserSensorService
	{
		private readonly DataContext dataContext;

		public UserSensorService(DataContext dataContext)
		{
			this.dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
		}

		public async Task<IPagedList<UserSensor>> FilterUserSensorsAsync(string filter = "", int pageNumber = 1, int pageSize = 10)
		{
			Validator.ValidateNull(filter, "Filter cannot be null!");

			Validator.ValidateMinRange(pageNumber, 1, "Page number cannot be less then 1!");
			Validator.ValidateMinRange(pageSize, 0, "Page size cannot be less then 0!");

			var query = this.dataContext.UserSensors
				.Where(t => t.Name.Contains(filter));
			
			return await query.ToPagedListAsync(pageNumber, pageSize);
		}

		public async Task<IPagedList<UserSensor>> GetSensorsByUserId(Guid userId, int pageNumber = 1, int pageSize = 10)
		{
			Validator.ValidateMinRange(pageNumber, 1, "Page number cannot be less then 1!");
			Validator.ValidateMinRange(pageSize, 0, "Page size cannot be less then 0!");

			var query = this.dataContext.UserSensors
				.Where(us => us.UserId.Equals(userId));

			return await query.ToPagedListAsync(pageNumber, pageSize);
		}

		public async Task<UserSensor> GetSensorByIdAsync(string id)
		{
			var userSensor = await this.dataContext.UserSensors
				.FirstOrDefaultAsync(us => us.Id.Equals(Guid.Parse(id)));

			return userSensor;
		}

		public async Task<UserSensor> AddUserSensorAsync(string userId, string sensorId, string name, string description, 
			int latitude, int longitude, int alarmMin, int alarmMax, int pollingInterval, bool alarmTriggered, bool isPublic )
		{
			Validator.ValidateNull(name, "Sensor name cannot be null!");

			if (await this.dataContext.UserSensors.AnyAsync(us => us.Name.Equals(name)))
			{
				throw new EntityAlreadyExistsException("User sensor already exists!");
			}
			
			var userSensor = new UserSensor
			{
				UserId = Guid.Parse(userId),
				Name = name,
				Description = description,
				Latitude = latitude,
				Longitude = longitude,
				AlarmTriggered = alarmTriggered,
				AlarmMin = alarmMin,
				AlarmMax = alarmMax,
				IsPublic = isPublic,
				PollingInterval = pollingInterval
			};

			userSensor.SensorId = Guid.Parse(sensorId);

			await this.dataContext.UserSensors.AddAsync(userSensor);
			await this.dataContext.SaveChangesAsync();

			return userSensor;
		}

		public async Task UpdateUserSensorAsync(UserSensor userSensor)
		{
			this.dataContext.Update(userSensor);
			await this.dataContext.SaveChangesAsync();
		}
	}
}
