using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services.Contracts;
using SD.Web.Areas.Administration.Controllers;
using SD.Web.Areas.Administration.Models.SensorViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace SD.Web.Tests.Areas.Administration.SensorControllerTests
{
	[TestClass]
	public class IndexAction_Should
	{
		[TestMethod]
		public async Task ReturnViewResult_WhenCalled()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			IMemoryCache memoryCacheMock = new MemoryCache(new MemoryCacheOptions());
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();
			
			string validFilter = string.Empty;
			int validPageNumber = 1;
			int validPageSize = 10;

			IPagedList<UserSensor> userSensors = new PagedList<UserSensor>(new List<UserSensor>().AsQueryable(), validPageNumber, validPageSize);

			userSensorServiceMock.Setup(mock => mock.FilterUserSensorsAsync(validFilter, validPageNumber, validPageSize))
				.Returns(Task.FromResult(userSensors));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock,
				sensorDataServiceMock.Object);

			// Act
			var result = await SUT.Index();

			// Assert
			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}

		[TestMethod]
		public async Task ReturnCorrectViewModel_WhenCalled()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			IMemoryCache memoryCacheMock = new MemoryCache(new MemoryCacheOptions());
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validFilter = string.Empty;
			int validPageNumber = 1;
			int validPageSize = 10;

			IPagedList<UserSensor> userSensors = new PagedList<UserSensor>(new List<UserSensor>().AsQueryable(), validPageNumber, validPageSize);

			userSensorServiceMock.Setup(mock => mock.FilterUserSensorsAsync(validFilter, validPageNumber, validPageSize))
				.Returns(Task.FromResult(userSensors));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock,
				sensorDataServiceMock.Object);

			// Act
			var result = await SUT.Index() as ViewResult;

			// Assert
			Assert.IsInstanceOfType(result.Model, typeof(SensorIndexViewModel));
		}

		[TestMethod]
		public async Task CallGetSensorsByUserId_WhenInvoked()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validId = "testGuid";
			double validLastValue = 10;
			int validPageNumber = 1;
			int validPageSize = 10;

			UserSensor validUserSensor = new UserSensor();
			Sensor validSensor = new Sensor();
			validUserSensor.Sensor = validSensor;
			validUserSensor.Sensor.LastValue = validLastValue.ToString();
			List<UserSensor> validList = new List<UserSensor>
			{
				validUserSensor
			};
			IPagedList<UserSensor> validCollection = new PagedList<UserSensor>(validList.AsQueryable(), validPageNumber, validPageSize);

			userSensorServiceMock.Setup(mock => mock.GetSensorsByUserId(validId, validPageNumber, validPageSize))
				.Returns(Task.FromResult(validCollection));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act
			await SUT.Index(validId, validPageNumber, validPageSize);

			// Assert
			userSensorServiceMock
				.Verify(mock => mock.GetSensorsByUserId(validId, validPageNumber, validPageSize), 
				Times.Once);
		}

		[TestMethod]
		public async Task ThrowApplicationException_WhenPassedNullId()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validId = "testGuid";
			double validLastValue = 10;
			int validPageNumber = 1;
			int validPageSize = 10;

			UserSensor validUserSensor = new UserSensor();
			Sensor validSensor = new Sensor();
			validUserSensor.Sensor = validSensor;
			validUserSensor.Sensor.LastValue = validLastValue.ToString();
			List<UserSensor> validList = new List<UserSensor>
			{
				validUserSensor
			};
			IPagedList<UserSensor> validCollection = new PagedList<UserSensor>(validList.AsQueryable(), validPageNumber, validPageSize);

			userSensorServiceMock.Setup(mock => mock.GetSensorsByUserId(validId, validPageNumber, validPageSize))
				.Returns(Task.FromResult(validCollection));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act & Assert
			await Assert.ThrowsExceptionAsync<ApplicationException>(() =>
				SUT.Index(null, validPageNumber, validPageSize));
		}

		[TestMethod]
		public async Task ThrowApplicationException_WhenPassedUserSensorCollectionIsEmpty()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validId = "testGuid";
			int validPageNumber = 1;
			int validPageSize = 10;

			List<UserSensor> emptyList = new List<UserSensor>();
			IPagedList<UserSensor> validCollection = new PagedList<UserSensor>(emptyList.AsQueryable(), validPageNumber, validPageSize);

			userSensorServiceMock.Setup(mock => mock.GetSensorsByUserId(validId, validPageNumber, validPageSize))
				.Returns(Task.FromResult(validCollection));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act & Assert
			await Assert.ThrowsExceptionAsync<ApplicationException>(() =>
				SUT.Index(validId, validPageNumber, validPageSize));
		}
	}
}
