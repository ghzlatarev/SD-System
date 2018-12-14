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
    public class FilterUserSensors_Should
    {
        [TestMethod]
        public async Task ThrowArgumentNullException_WhenPassedNullFilter()
        {
            // Arrange
            Mock<DataContext> dataContextMock = new Mock<DataContext>();
            Mock<ISensorService> sensor = new Mock<ISensorService>();

            string invalidFilter = null;
            int validPageNumber = 1;
            int validPageSize = 10;

            UserSensorService SUT = new UserSensorService(
                dataContextMock.Object,
                sensor.Object);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => SUT.FilterUserSensorsAsync(invalidFilter, validPageNumber, validPageSize));
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-10)]
        public async Task ThrowArgumentOutOfRangeException_WhenPassedInvalidPageNumber(int invalidPageNumber)
        {
            // Arrange
            Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
            Mock<DataContext> dataContextMock = new Mock<DataContext>();

            
            string validFilter = string.Empty;
            int validPageSize = 10;


            UserSensorService SUT = new UserSensorService(
                dataContextMock.Object,
                sensorServiceMock.Object);
            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(
                () => SUT.FilterUserSensorsAsync(validFilter, invalidPageNumber, validPageSize));
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(-10)]
        public async Task ThrowArgumentOutOfRangeException_WhenPassedInvalidPageSize(int invalidPageSize)
        {
            // Arrange
            Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
            Mock<DataContext> dataContextMock = new Mock<DataContext>();
            
            string validFilter = string.Empty;
            int validPageNumber = 1;

            UserSensorService SUT = new UserSensorService(
                dataContextMock.Object,
                sensorServiceMock.Object);
            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(
                () => SUT.FilterUserSensorsAsync(validFilter, validPageNumber, invalidPageSize));
        }
    }
}
