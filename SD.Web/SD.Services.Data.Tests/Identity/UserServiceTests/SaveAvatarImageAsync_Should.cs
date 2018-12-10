using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Context;
using SD.Services.Data.Services.Identity;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SD.Services.Data.Tests.Identity.UserServiceTests
{
	[TestClass]
    public class SaveAvatarImageAsync_Should
    {
        [TestMethod]
        public async Task ThrowArgumentNullException_WhenPassedNullStream()
        {
            // Arrange
            Mock<DataContext> dataContextMock = new Mock<DataContext>();

            UserService SUT = new UserService(dataContextMock.Object);
            string validUserId = Guid.NewGuid().ToString();

            // Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => SUT.SaveAvatarImageAsync(null, validUserId));
        }

        [TestMethod]
        public async Task ThrowArgumentNullException_WhenPassedNullUserId()
        {
            // Arrange
            Mock<DataContext> dataContextMock = new Mock<DataContext>();
            Mock<Stream> validStreamMock = new Mock<Stream>();

            UserService SUT = new UserService(dataContextMock.Object);

            // Act
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => SUT.SaveAvatarImageAsync(validStreamMock.Object, null));
        }
    }
}
