using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services.Contracts;
using SD.Web.Areas.Administration.Controllers;
using SD.Web.Areas.Administration.Models.SensorViewModels;
using SD.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace SD.Web.Tests.Areas.Administration.SensorControllerTests
{
	[TestClass]
	public class FilterAction_Should
	{
		[TestMethod]
		public async Task ReturnPartialViewResult_WhenInvoked()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validFilter = string.Empty;
			int validPageNumber = 1;
			int validPageSize = 10;
			
			IPagedList<UserSensor> userSensors = new PagedList<UserSensor>(new List<UserSensor>().AsQueryable(), validPageNumber, validPageSize);

			userSensorServiceMock.Setup(mock => mock.FilterUserSensorsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
				.Returns(Task.FromResult(userSensors));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act
			var result = await SUT.Filter(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());

			// Assert
			Assert.IsInstanceOfType(result, typeof(PartialViewResult));
		}

		[TestMethod]
		public async Task ReturnCorrectViewModel_WhenInvoked()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validFilter = string.Empty;
			int validPageNumber = 1;
			int validPageSize = 10;

			IPagedList<UserSensor> userSensors = new PagedList<UserSensor>(new List<UserSensor>().AsQueryable(), validPageNumber, validPageSize);

			userSensorServiceMock.Setup(mock => mock.FilterUserSensorsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
				.Returns(Task.FromResult(userSensors));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act
			var result = await SUT.Filter(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()) as PartialViewResult;

			// Assert
			Assert.IsInstanceOfType(result.Model, typeof(TableViewModel<SensorTableViewModel>));
		}

		[TestMethod]
		public async Task CallFilterSensorsAsync_WhenInvoked()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validFilter = string.Empty;
			int validPageNumber = 1;
			int validPageSize = 10;

			IPagedList<UserSensor> userSensors = new PagedList<UserSensor>(new List<UserSensor>().AsQueryable(), validPageNumber, validPageSize);

			userSensorServiceMock.Setup(mock => mock.FilterUserSensorsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
				.Returns(Task.FromResult(userSensors));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act
			await SUT.Filter(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());

			// Assert
			userSensorServiceMock
				.Verify(mock => mock.FilterUserSensorsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()),
				Times.Once);
		}
	}
}
