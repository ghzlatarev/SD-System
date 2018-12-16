using Microsoft.AspNetCore.SignalR;
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

namespace SD.Services.Data.Tests.NotificationServiceTests
{
    [TestClass]
    public class Constructor_Should
    {
        [TestMethod]
        public void ThrowArgumentNullException_WhenNullHubClient()
        {
            // Arrange

            Mock<DataContext> dataContextMock = new Mock<DataContext>();
            Mock<IHubContext<NotificationHub>> hubMock = new Mock<IHubContext<NotificationHub>>();


            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(
                () => new NotificationService(null, dataContextMock.Object)
                );
        }

        [TestMethod]
        public void ThrowArgumentNullException_WhenNullDataContext()
        {

            Mock<DataContext> dataContextMock = new Mock<DataContext>();
            Mock<IHubContext<NotificationHub>> hubMock = new Mock<IHubContext<NotificationHub>>();


            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(
                () => new NotificationService(hubMock.Object, null)
                );
        }

    }
}
