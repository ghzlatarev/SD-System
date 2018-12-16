using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Context;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services;
using System;
using System.Threading.Tasks;

namespace SD.Services.Data.Tests.NotificationServiceTests
{
    [TestClass]
    public class GetItemsAsync_Should
    {
        [TestMethod]
        public async Task ReturnListNotifications_WhenUserIdValid()
        {
            // Arrange
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
    .UseInMemoryDatabase(databaseName: "ReturnListNotifications_WhenUserIdValid")
    .Options;

            var validNotification1 = new Notifications
            {
                Message = "message 1",
                UserId = "0015cc77-e65f-4bc7-8921-55a88c6b283b"
            };

            var validNotification2 = new Notifications
            {
                Message = "message 2",
                UserId = "0015cc77-e65f-4bc7-8921-55a88c6b283c"
            };

            using (DataContext actContext = new DataContext(contextOptions))
            {
                Mock<IHubContext<NotificationHub>> hubMock = new Mock<IHubContext<NotificationHub>>();

                await actContext.Notifications.AddAsync(validNotification1);
                await actContext.Notifications.AddAsync(validNotification2);
                await actContext.SaveChangesAsync();
            }

            using (DataContext assertContext = new DataContext(contextOptions))
            {
                Mock<IHubContext<NotificationHub>> hubMock = new Mock<IHubContext<NotificationHub>>();
                var SUT = new NotificationService(hubMock.Object, assertContext);
                var list = SUT.GetItemsAsync("0015cc77-e65f-4bc7-8921-55a88c6b283b").Result;
                Assert.IsTrue(list.Count == 1);
                Assert.IsTrue(list[0].Message == "message 1");
            }
            

        }
        
    }
}
