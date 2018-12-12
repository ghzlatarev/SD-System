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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD.Services.Data.Tests.SensorDataServiceTests
{
	[TestClass]
	public class GetSensorsData_Should
	{
		[TestMethod]
		public async Task CallGetOrSetCache_WhenInvoked()
		{
			Mock<IApiClient> apiClientMock = new Mock<IApiClient>();
			Mock<DataContext> dataContextMock = new Mock<DataContext>();
			Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCacheWrapper> memoryCacheMock = new Mock<IMemoryCacheWrapper>();

			IEnumerable<Sensor> expectedList = new List<Sensor>();

			memoryCacheMock.Setup(mock => mock.GetOrSetCache())
				.Returns(Task.FromResult(expectedList));

			SensorDataService SUT = new SensorDataService(
				apiClientMock.Object,
				dataContextMock.Object,
				notificationServiceMock.Object,
				memoryCacheMock.Object);

			await SUT.GetSensorsDataAsync();

			memoryCacheMock.Verify(mock => mock.GetOrSetCache(), 
				Times.Once);
		}
	}
}
