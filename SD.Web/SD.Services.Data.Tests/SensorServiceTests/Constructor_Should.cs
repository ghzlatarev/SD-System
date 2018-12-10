using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Context;
using SD.Services.Data.Services;
using SD.Services.External;
using System;

namespace SD.Services.Data.Tests.SensorServiceTests
{
	[TestClass]
	public class Constructor_Should
	{
		[TestMethod]
		public void ThrowArgumentNullException_WhenNullDataContext()
		{
			// Arrange
			Mock<IApiClient> apiClientMock = new Mock<IApiClient>();

			// Act & Assert
			Assert.ThrowsException<ArgumentNullException>(
				() => new SensorService(
					apiClientMock.Object,
					null));
		}

		[TestMethod]
		public void ThrowArgumentNullException_WhenNullPandaScoreClient()
		{
			// Arrange
			Mock<DataContext> dataContextMock = new Mock<DataContext>();

			// Act & Assert
			Assert.ThrowsException<ArgumentNullException>(
				() => new SensorService(
					null,
					dataContextMock.Object));
		}
	}
}
