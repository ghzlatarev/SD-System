using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Context;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services;
using SD.Services.Data.Services.Contracts;
using SD.Services.Data.Wrappers.Contracts;
using SD.Services.External;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SD.Services.Data.Tests.SensorDataServiceTests
{
	[TestClass]
	public class WithinMemory
	{
		[TestMethod]
		public async Task CallGetSensorData_WhenTimeSpanIsMoreThanPollingInterval()
		{
			// Arrange
			var contextOptions = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: "CallGetSensorData_WhenTimeSpanIsMoreThanPollingInterval")
				.Options;

			Sensor validSensor = new Sensor
			{
				SensorId = "validId"
			};
			DateTime value = new DateTime(2017, 1, 18);
			validSensor.LastTimeStamp = value;
			validSensor.MinPollingIntervalInSeconds = 0;
			IEnumerable<Sensor> expectedList = new List<Sensor>
			{
				validSensor
			};
			SensorData dbSensorData = new SensorData
			{
				Sensor = validSensor
			};
			UserSensor dbUserSensor = new UserSensor
			{
				Sensor = validSensor
			};

			using (DataContext actContext = new DataContext(contextOptions))
			{
				Mock<IApiClient> apiClientMock = new Mock<IApiClient>();
				Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();
				Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
				Mock<IMemoryCacheWrapper> memoryCacheMock = new Mock<IMemoryCacheWrapper>();

				await actContext.SensorData.AddAsync(dbSensorData);
				await actContext.Sensors.AddAsync(validSensor);
				await actContext.UserSensors.AddAsync(dbUserSensor);

				await actContext.SaveChangesAsync();

				memoryCacheMock.Setup(mock => mock.GetOrSetCache())
				.Returns(Task.FromResult(expectedList));

				SensorDataService SUT = new SensorDataService(
					apiClientMock.Object,
					actContext,
					notificationServiceMock.Object,
					memoryCacheMock.Object);

				await SUT.GetSensorsDataAsync();

				apiClientMock.Verify(mock => mock.GetSensorData(It.IsAny<string>()),
								Times.Once);

			}
		}

		[TestMethod]
		public async Task NotCallGetSensorData_WhenTimeSpanIsLessThanPollingInterval()
		{
			// Arrange
			var contextOptions = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: "NotCallGetSensorData_WhenTimeSpanIsLessThanPollingInterval")
				.Options;

			Sensor validSensor = new Sensor
			{
				SensorId = "validId"
			};
			DateTime value = DateTime.Now;
			validSensor.LastTimeStamp = value;
			validSensor.MinPollingIntervalInSeconds = 1000000;
			IEnumerable<Sensor> expectedList = new List<Sensor>
			{
				validSensor
			};
			SensorData dbSensorData = new SensorData
			{
				Sensor = validSensor
			};
			UserSensor dbUserSensor = new UserSensor
			{
				Sensor = validSensor
			};

			using (DataContext actContext = new DataContext(contextOptions))
			{
				Mock<IApiClient> apiClientMock = new Mock<IApiClient>();
				Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();
				Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
				Mock<IMemoryCacheWrapper> memoryCacheMock = new Mock<IMemoryCacheWrapper>();

				await actContext.SensorData.AddAsync(dbSensorData);
				await actContext.Sensors.AddAsync(validSensor);
				await actContext.UserSensors.AddAsync(dbUserSensor);

				await actContext.SaveChangesAsync();

				memoryCacheMock.Setup(mock => mock.GetOrSetCache())
				.Returns(Task.FromResult(expectedList));

				SensorDataService SUT = new SensorDataService(
					apiClientMock.Object,
					actContext,
					notificationServiceMock.Object,
					memoryCacheMock.Object);

				await SUT.GetSensorsDataAsync();

				apiClientMock.Verify(mock => mock.GetSensorData(It.IsAny<string>()),
								Times.Never);
			}
		}

		[TestMethod]
		public async Task CallCheckAlarmNotifications_WhenTimeSpanIsLessThanPollingInterval()
		{
			// Arrange
			var contextOptions = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: "CallCheckAlarmNotifications_WhenTimeSpanIsLessThanPollingInterval")
				.Options;

			Sensor validSensor = new Sensor
			{
				SensorId = "validId",
				UserSensors = new List<UserSensor>()
			};
			DateTime value = new DateTime(2017, 1, 18);
			validSensor.LastTimeStamp = value;
			validSensor.MinPollingIntervalInSeconds = 0;
			IEnumerable<Sensor> expectedList = new List<Sensor>
			{
				validSensor
			};
			SensorData dbSensorData = new SensorData
			{
				Sensor = validSensor
			};
			UserSensor dbUserSensor = new UserSensor
			{
				Sensor = validSensor
			};
			validSensor.UserSensors.Add(dbUserSensor);
			List<UserSensor> affectedSensors = new List<UserSensor>();
			affectedSensors.AddRange(validSensor.UserSensors);
			SensorData apiSensorData = new SensorData();
			apiSensorData.Value = "1";
			apiSensorData.TimeStamp = DateTime.Now;

			using (DataContext actContext = new DataContext(contextOptions))
			{
				Mock<IApiClient> apiClientMock = new Mock<IApiClient>();
				Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();
				Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
				Mock<IMemoryCacheWrapper> memoryCacheMock = new Mock<IMemoryCacheWrapper>();

				await actContext.SensorData.AddAsync(dbSensorData);
				await actContext.Sensors.AddAsync(validSensor);
				await actContext.UserSensors.AddAsync(dbUserSensor);

				await actContext.SaveChangesAsync();

				memoryCacheMock.Setup(mock => mock.GetOrSetCache())
				.Returns(Task.FromResult(expectedList));

				apiClientMock.Setup(mock => mock.GetSensorData(It.IsAny<string>()))
					.Returns(Task.FromResult(apiSensorData));

				notificationServiceMock.Setup(mock => mock.CheckAlarmNotificationsAsync(affectedSensors))
					.Returns(Task.CompletedTask);

				SensorDataService SUT = new SensorDataService(
					apiClientMock.Object,
					actContext,
					notificationServiceMock.Object,
					memoryCacheMock.Object);

				await SUT.GetSensorsDataAsync();

				notificationServiceMock.Verify(mock => mock.CheckAlarmNotificationsAsync(affectedSensors),
								Times.Once);
			}
		}

		[TestMethod]
		public async Task UpdateSensorValues_WhenTimeSpanIsLessThanPollingInterval_AndNewDataIsOk()
		{
			// Arrange
			var contextOptions = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: "UpdateSensorData_WhenTimeSpanIsLessThanPollingInterval")
				.Options;

			Sensor validSensor = new Sensor
			{
				SensorId = "validId",
				UserSensors = new List<UserSensor>()
			};
			DateTime value = new DateTime(2017, 1, 18);
			validSensor.LastTimeStamp = value;
			validSensor.MinPollingIntervalInSeconds = 0;
			IEnumerable<Sensor> expectedList = new List<Sensor>
			{
				validSensor
			};
			SensorData dbSensorData = new SensorData
			{
				Sensor = validSensor
			};
			UserSensor dbUserSensor = new UserSensor
			{
				Sensor = validSensor
			};
			validSensor.UserSensors.Add(dbUserSensor);
			List<UserSensor> affectedSensors = new List<UserSensor>();
			affectedSensors.AddRange(validSensor.UserSensors);
			SensorData apiSensorData = new SensorData();
			apiSensorData.Value = "1";
			apiSensorData.TimeStamp = DateTime.Now;

			using (DataContext actContext = new DataContext(contextOptions))
			{
				Mock<IApiClient> apiClientMock = new Mock<IApiClient>();
				Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();
				Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
				Mock<IMemoryCacheWrapper> memoryCacheMock = new Mock<IMemoryCacheWrapper>();

				await actContext.SensorData.AddAsync(dbSensorData);
				await actContext.Sensors.AddAsync(validSensor);
				await actContext.UserSensors.AddAsync(dbUserSensor);

				await actContext.SaveChangesAsync();

				memoryCacheMock.Setup(mock => mock.GetOrSetCache())
				.Returns(Task.FromResult(expectedList));

				apiClientMock.Setup(mock => mock.GetSensorData(It.IsAny<string>()))
					.Returns(Task.FromResult(apiSensorData));

				notificationServiceMock.Setup(mock => mock.CheckAlarmNotificationsAsync(affectedSensors))
					.Returns(Task.CompletedTask);

				SensorDataService SUT = new SensorDataService(
					apiClientMock.Object,
					actContext,
					notificationServiceMock.Object,
					memoryCacheMock.Object);

				await SUT.GetSensorsDataAsync();
			}

			// Assert
			using (DataContext assertContext = new DataContext(contextOptions))
			{
				var updatedSensorData = await assertContext.SensorData.FirstAsync();
				var updatedSensor = await assertContext.Sensors.FirstAsync();

				Assert.IsTrue(updatedSensorData.Value == "1");
				Assert.IsTrue(updatedSensor.LastValue == "1");
			}
		}

		[TestMethod]
		public async Task NotUpdateSensorValues_WhenTimeSpanIsLessThanPollingInterval_AndNewDataIsNot200Ok()
		{
			// Arrange
			var contextOptions = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: "UpdateSensorData_WhenTimeSpanIsLessThanPollingInterval")
				.Options;

			Sensor validSensor = new Sensor
			{
				SensorId = "validId",
				LastValue = "1",
				UserSensors = new List<UserSensor>()
			};
			DateTime value = new DateTime(2017, 1, 18);
			validSensor.LastTimeStamp = value;
			validSensor.MinPollingIntervalInSeconds = 0;
			IEnumerable<Sensor> expectedList = new List<Sensor>
			{
				validSensor
			};
			SensorData dbSensorData = new SensorData
			{
				Sensor = validSensor
			};
			UserSensor dbUserSensor = new UserSensor
			{
				Sensor = validSensor
			};
			validSensor.UserSensors.Add(dbUserSensor);
			List<UserSensor> affectedSensors = new List<UserSensor>();
			affectedSensors.AddRange(validSensor.UserSensors);
			SensorData apiSensorData = null;

			using (DataContext actContext = new DataContext(contextOptions))
			{
				Mock<IApiClient> apiClientMock = new Mock<IApiClient>();
				Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();
				Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
				Mock<IMemoryCacheWrapper> memoryCacheMock = new Mock<IMemoryCacheWrapper>();

				await actContext.SensorData.AddAsync(dbSensorData);
				await actContext.Sensors.AddAsync(validSensor);
				await actContext.UserSensors.AddAsync(dbUserSensor);

				await actContext.SaveChangesAsync();

				memoryCacheMock.Setup(mock => mock.GetOrSetCache())
				.Returns(Task.FromResult(expectedList));

				apiClientMock.Setup(mock => mock.GetSensorData(It.IsAny<string>()))
					.Returns(Task.FromResult(apiSensorData));

				notificationServiceMock.Setup(mock => mock.CheckAlarmNotificationsAsync(affectedSensors))
					.Returns(Task.CompletedTask);

				SensorDataService SUT = new SensorDataService(
					apiClientMock.Object,
					actContext,
					notificationServiceMock.Object,
					memoryCacheMock.Object);

				await SUT.GetSensorsDataAsync();
			}

			// Assert
			using (DataContext assertContext = new DataContext(contextOptions))
			{
				var nonUpdatedSensorData = await assertContext.SensorData.FirstAsync();
				var nonUpdatedSensor = await assertContext.Sensors.FirstAsync();

				Assert.IsTrue(nonUpdatedSensorData.Value == "1");
				Assert.IsTrue(nonUpdatedSensor.LastValue == "1");
			}
		}
	}
}
