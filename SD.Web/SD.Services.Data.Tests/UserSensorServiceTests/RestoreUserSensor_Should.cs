using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Context;
using SD.Services.Data.Services;
using SD.Services.Data.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SD.Services.Data.Tests.UserSensorServiceTests
{
	[TestClass]
	public class RestoreUserSensor_Should
	{
		[TestMethod]
		public async Task ThrowArgumentNullException_WhenPassedNullId()
		{
			// Arrange
			Mock<DataContext> dataContextMock = new Mock<DataContext>();
			Mock<ISensorService> sensorServicetMock = new Mock<ISensorService>();

			// Act
			UserSensorService SUT = new UserSensorService(
				   dataContextMock.Object,
				   sensorServicetMock.Object);

			// Assert
			await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
			   await SUT.RestoreUserSensor(null));
		}

		[DataTestMethod]
		[DataRow("invalidId")]
		[DataRow("123231-321321-ewqewq")]
		public async Task ThrowArgumentException_WhenPassedInvalidId(string invalidId)
		{
			// Arrange
			Mock<DataContext> dataContextMock = new Mock<DataContext>();
			Mock<ISensorService> sensorServicetMock = new Mock<ISensorService>();

			// Act
			UserSensorService SUT = new UserSensorService(
				   dataContextMock.Object,
				   sensorServicetMock.Object);

			// Assert
			await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
			   await SUT.RestoreUserSensor(invalidId));
		}
	}
}
