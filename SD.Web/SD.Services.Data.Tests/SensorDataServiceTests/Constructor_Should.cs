using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Context;
using SD.Services.Data.Services;
using SD.Services.Data.Services.Contracts;
using SD.Services.Data.Wrappers.Contracts;
using SD.Services.External;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Services.Data.Tests.SensorDataServiceTests
{
	[TestClass]
	public class Constructor_Should
	{
		[TestMethod]
		public void ThrowArgumentNullException_WhenNullApiClient()
		{
			// Arrange
			Mock<IApiClient> apiClientMock = new Mock<IApiClient>();
			Mock<DataContext> dataContextMock = new Mock<DataContext>();
			Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();
			Mock<IMemoryCacheWrapper> memoryCacheMock = new Mock<IMemoryCacheWrapper>();

			// Act & Assert
			Assert.ThrowsException<ArgumentNullException>(
				() => new SensorDataService(
					null,
					dataContextMock.Object,
					notificationServiceMock.Object,
					memoryCacheMock.Object));
		}

		[TestMethod]
		public void ThrowArgumentNullException_WhenNullDataContext()
		{
			// Arrange
			Mock<IApiClient> apiClientMock = new Mock<IApiClient>();
			Mock<DataContext> dataContextMock = new Mock<DataContext>();
			Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();
			Mock<IMemoryCacheWrapper> memoryCacheMock = new Mock<IMemoryCacheWrapper>();

			// Act & Assert
			Assert.ThrowsException<ArgumentNullException>(
				() => new SensorDataService(
					apiClientMock.Object,
					null,
					notificationServiceMock.Object,
					memoryCacheMock.Object));
		}

		[TestMethod]
		public void ThrowArgumentNullException_WhenNullNotificationService()
		{
			// Arrange
			Mock<IApiClient> apiClientMock = new Mock<IApiClient>();
			Mock<DataContext> dataContextMock = new Mock<DataContext>();
			Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();
			Mock<IMemoryCacheWrapper> memoryCacheMock = new Mock<IMemoryCacheWrapper>();

			// Act & Assert
			Assert.ThrowsException<ArgumentNullException>(
				() => new SensorDataService(
					apiClientMock.Object,
					dataContextMock.Object,
					null,
					memoryCacheMock.Object));
		}

		[TestMethod]
		public void ThrowArgumentNullException_WhenNullMemoryCache()
		{
			// Arrange
			Mock<IApiClient> apiClientMock = new Mock<IApiClient>();
			Mock<DataContext> dataContextMock = new Mock<DataContext>();
			Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();

			// Act & Assert
			Assert.ThrowsException<ArgumentNullException>(
				() => new SensorDataService(
					apiClientMock.Object,
					dataContextMock.Object,
					notificationServiceMock.Object,
					null));
		}
	}
}
