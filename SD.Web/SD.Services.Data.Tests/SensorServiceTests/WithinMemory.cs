using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Context;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services;
using SD.Services.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Services.Data.Tests.SensorServiceTests
{
    [TestClass]
	public class WithInMemory
	{
		[TestMethod]
		public async Task ListSensors_ShouldReturnCollectionOfSensors()
		{
			// Arrange
			var contextOptions = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: "ListSensors_ShouldReturnCollectionOfSensors")
				.Options;

			string validId = Guid.NewGuid().ToString();
			string validTag = "testTag";
			string validLastValue = 10.ToString();
			string validSensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e";

			Sensor validSensor = new Sensor
			{
				Id = validId,
				Tag = validTag,
				LastValue = validLastValue,
				SensorId = validSensorId
			};

			IEnumerable<Sensor> result = new List<Sensor>();

			// Act
			using (DataContext actContext = new DataContext(contextOptions))
			{
				Mock<IApiClient> apiClientMock = new Mock<IApiClient>();

				await actContext.Sensors.AddAsync(validSensor);
				await actContext.SaveChangesAsync();

				SensorService SUT = new SensorService(
					apiClientMock.Object,
					actContext);

				result = await SUT.ListSensorsAsync();
			}

			// Assert
			using (DataContext assertContext = new DataContext(contextOptions))
			{
				var sensor = result.ToArray()[0];

				Assert.IsTrue(assertContext.Sensors.Count().Equals(result.Count()));
				Assert.IsTrue(assertContext.Sensors.Any(s => s.Tag.Equals(sensor.Tag)));
				Assert.IsTrue(assertContext.Sensors.Any(s => s.LastValue.Equals(sensor.LastValue)));
				Assert.IsTrue(assertContext.Sensors.Any(s => s.SensorId.Equals(sensor.SensorId)));
			}
		}

		[TestMethod]
		public async Task ListStateSensors_ShouldReturnCollectionOfStateSensors()
		{
			// Arrange
			var contextOptions = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: "ListStateSensors_ShouldReturnCollectionOfStateSensors")
				.Options;

			string validId = Guid.NewGuid().ToString();
			string validSensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e";
			bool validStateBool = true;

			Sensor validSensor = new Sensor
			{
				Id = validId,
				SensorId = validSensorId,
				IsState = validStateBool
			};

			IEnumerable<Sensor> result = new List<Sensor>();

			// Act
			using (DataContext actContext = new DataContext(contextOptions))
			{
				Mock<IApiClient> apiClientMock = new Mock<IApiClient>();

				await actContext.Sensors.AddAsync(validSensor);
				await actContext.SaveChangesAsync();

				SensorService SUT = new SensorService(
					apiClientMock.Object,
					actContext);

				result = await SUT.ListSensorsAsync();
			}

			// Assert
			using (DataContext assertContext = new DataContext(contextOptions))
			{
				var sensor = result.ToArray()[0];
				
				Assert.IsTrue(assertContext.Sensors.Any(s => s.IsState.Equals(sensor.IsState)));
			}
		}

		[TestMethod]
		public async Task ListNonStateSensors_ShouldReturnCollectionOfNonStateSensors()
		{
			// Arrange
			var contextOptions = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: "ListNonStateSensors_ShouldReturnCollectionOfNonStateSensors")
				.Options;

			string validId = Guid.NewGuid().ToString();
			string validSensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e";
			bool validStateBool = false;

			Sensor validSensor = new Sensor
			{
				Id = validId,
				SensorId = validSensorId,
				IsState = validStateBool
			};

			IEnumerable<Sensor> result = new List<Sensor>();

			// Act
			using (DataContext actContext = new DataContext(contextOptions))
			{
				Mock<IApiClient> apiClientMock = new Mock<IApiClient>();

				await actContext.Sensors.AddAsync(validSensor);
				await actContext.SaveChangesAsync();

				SensorService SUT = new SensorService(
					apiClientMock.Object,
					actContext);

				result = await SUT.ListSensorsAsync();
			}

			// Assert
			using (DataContext assertContext = new DataContext(contextOptions))
			{
				var sensor = result.ToArray()[0];

				Assert.IsTrue(assertContext.Sensors.Any(s => s.IsState.Equals(sensor.IsState)));
			}
		}

		[TestMethod]
		public async Task GetSensorByIdAsync_ShouldReturnSensor_WhenPassedValidParameters()
		{
			// Arrange
			var contextOptions = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: "GetSensorByIdAsync_ShouldReturnSensor_WhenPassedValidParameters")
				.Options;

			string validId = Guid.NewGuid().ToString();
			string validSensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e";
			string validTag = "testTag";

			Sensor validSensor = new Sensor
			{
				Id = validId,
				SensorId = validSensorId,
				Tag = validTag
			};

			Sensor result = null;

			// Act
			using (DataContext actContext = new DataContext(contextOptions))
			{
				Mock<IApiClient> apiClientMock = new Mock<IApiClient>();

				await actContext.Sensors.AddAsync(validSensor);
				await actContext.SaveChangesAsync();

				SensorService SUT = new SensorService(
					apiClientMock.Object,
					actContext);

				result = await SUT.GetSensorByIdAsync(validId);
			}

			// Assert
			using (DataContext assertContext = new DataContext(contextOptions))
			{
				Assert.IsTrue(assertContext.Sensors.Any(s => s.Id.Equals(result.Id)));
				Assert.IsTrue(assertContext.Sensors.Any(s => s.SensorId.Equals(result.SensorId)));
				Assert.IsTrue(assertContext.Sensors.Any(s => s.Tag.Equals(result.Tag)));
			}
		}

		[TestMethod]
		public async Task RebaseSensorsAsync_ShouldRepopulateSensorsTable()
		{
			// Arrange
			var contextOptions = new DbContextOptionsBuilder<DataContext>()
				.UseInMemoryDatabase(databaseName: "RebaseSensorsAsync_ShouldRepopulateSensorsTable")
				.Options;
			
			string validSensorId = "81a2e1b1-ea5d-4356-8266-b6b42471653e";
			string validTarget = "all";

			Sensor validSensor = new Sensor
			{
				Id = Guid.NewGuid().ToString(),
				SensorId = validSensorId,
				Tag = "testTag",
				DeletedOn = DateTime.UtcNow.AddHours(2),
				IsDeleted = true
			};

			IEnumerable<Sensor> validSensorList = new List<Sensor>()
			{
				validSensor
			};

			// Act
			using (DataContext actContext = new DataContext(contextOptions))
			{
				Mock<IApiClient> apiClientMock = new Mock<IApiClient>();

				apiClientMock
					.Setup(mock => mock.GetApiSensors(validTarget))
					.Returns(Task.FromResult(validSensorList));

				SensorService SUT = new SensorService(
					apiClientMock.Object,
					actContext);

				await SUT.RebaseSensorsAsync();
			}

			// Assert
			using (DataContext assertContext = new DataContext(contextOptions))
			{
				Assert.IsTrue(assertContext.Sensors.Count() == 1);
				Assert.IsTrue(assertContext.Sensors.Contains(validSensor));
			}
		}
	}
}
