using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Services.Data.Services.Contracts;
using SD.Web.Areas.Administration.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Web.Tests.Areas.Administration.SensorControllerTests
{
	[TestClass]
	public class Constructor_Should
	{
		[TestMethod]
		public void ThrowArgumentNullException_WHenPassedNullUserSensorService()
		{
			// Arrange
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();
			
			// Act & Assert
			Assert.ThrowsException<ArgumentNullException>(() =>
				 new SensorController(
					 null,
					 sensorServiceMock.Object,
					 memoryCacheMock.Object,
					 sensorDataServiceMock.Object));
		}

		[TestMethod]
		public void ThrowArgumentNullException_WHenPassedNullSensorService()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();


			// Act & Assert
			Assert.ThrowsException<ArgumentNullException>(() =>
				 new SensorController(
					 userSensorServiceMock.Object,
					 null,
					 memoryCacheMock.Object,
					 sensorDataServiceMock.Object));
		}

		[TestMethod]
		public void ThrowArgumentNullException_WHenPassedNullMemoryCache()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();


			// Act & Assert
			Assert.ThrowsException<ArgumentNullException>(() =>
				 new SensorController(
					 userSensorServiceMock.Object,
					 sensorServiceMock.Object,
					 null,
					 sensorDataServiceMock.Object));
		}

		[TestMethod]
		public void ThrowArgumentNullException_WhenPassedNullSensorDataService()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();


			// Act & Assert
			Assert.ThrowsException<ArgumentNullException>(() =>
				 new SensorController(
					 userSensorServiceMock.Object,
					 sensorServiceMock.Object,
					 memoryCacheMock.Object,
					 null));
		}
	}
}
