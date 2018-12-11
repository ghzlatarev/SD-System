using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Models.Identity;
using SD.Services.Data.Services.Identity.Contracts;
using SD.Web.Areas.Admin.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SD.Web.Tests.Areas.Administration.UserControllerTests
{
	[TestClass]
	public class Constructor_Should
	{
		[TestMethod]
		public void ThrowArgumentNullException_WHenPassedNullUserService()
		{
			// Arrange
			var mockUserManager = new Mock<UserManager<ApplicationUser>>(
					new Mock<IUserStore<ApplicationUser>>().Object,
					new Mock<IOptions<IdentityOptions>>().Object,
					new Mock<IPasswordHasher<ApplicationUser>>().Object,
					new IUserValidator<ApplicationUser>[0],
					new IPasswordValidator<ApplicationUser>[0],
					new Mock<ILookupNormalizer>().Object,
					new Mock<IdentityErrorDescriber>().Object,
					new Mock<IServiceProvider>().Object,
					new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

			var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
					new Mock<IRoleStore<IdentityRole>>().Object,
					new IRoleValidator<IdentityRole>[0],
					new Mock<ILookupNormalizer>().Object,
					new Mock<IdentityErrorDescriber>().Object,
					new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

			// Act & Assert
			Assert.ThrowsException<ArgumentNullException>(() =>
				 new UserController(
					 null,
					 mockRoleManager.Object,
					 mockUserManager.Object));
		}

		[TestMethod]
		public void ThrowArgumentNullException_WHenPassedNullRoleManager()
		{
			// Arrange
			Mock<IUserService> userServiceMock = new Mock<IUserService>();
			var mockUserManager = new Mock<UserManager<ApplicationUser>>(
					new Mock<IUserStore<ApplicationUser>>().Object,
					new Mock<IOptions<IdentityOptions>>().Object,
					new Mock<IPasswordHasher<ApplicationUser>>().Object,
					new IUserValidator<ApplicationUser>[0],
					new IPasswordValidator<ApplicationUser>[0],
					new Mock<ILookupNormalizer>().Object,
					new Mock<IdentityErrorDescriber>().Object,
					new Mock<IServiceProvider>().Object,
					new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

			// Act & Assert
			Assert.ThrowsException<ArgumentNullException>(() =>
				 new UserController(
					 userServiceMock.Object,
					 null,
					 mockUserManager.Object));
		}

		[TestMethod]
		public void ThrowArgumentNullException_WHenPassedNullUserManager()
		{
			// Arrange
			Mock<IUserService> userServiceMock = new Mock<IUserService>();
			var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
					new Mock<IRoleStore<IdentityRole>>().Object,
					new IRoleValidator<IdentityRole>[0],
					new Mock<ILookupNormalizer>().Object,
					new Mock<IdentityErrorDescriber>().Object,
					new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

			// Act & Assert
			Assert.ThrowsException<ArgumentNullException>(() =>
				 new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 null));
		}
	}
}
