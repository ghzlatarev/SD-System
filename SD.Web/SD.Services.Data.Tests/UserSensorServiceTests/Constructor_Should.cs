using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Context;
using SD.Services.Data.Services;
using SD.Services.Data.Services.Contracts;
using SD.Services.External;
using System;

namespace SD.Services.Data.Tests.UserSensorServiceTests
{
    [TestClass]
    public class Constructor_Should
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullException_WhenNullDataContext()
        {
            // Arrange
            Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();

            // Act
            var userSensorService = new UserSensorService(null, sensorServiceMock.Object); 
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowArgumentNullException_WhenNullSensorService()
        {
            // Arrange
            Mock<DataContext> dataContextMock = new Mock<DataContext>();

            // Act
            var userSensorService = new UserSensorService(dataContextMock.Object, null);
        }
    }
}
