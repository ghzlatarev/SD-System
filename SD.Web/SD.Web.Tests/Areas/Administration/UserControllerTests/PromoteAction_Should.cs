using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Models.Identity;
using SD.Services.Data.Services.Identity.Contracts;
using SD.Web.Areas.Admin.Controllers;
using SD.Web.Areas.Administration.Models.UserViewModels;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SD.Web.Tests.Areas.Administration.UserControllerTests
{
	[TestClass]
	public class PromoteAction_Should
	{
		[TestMethod]
		public async Task ReturnPartialViewResult_WhenInvoked()
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

			var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
					new Mock<IRoleStore<IdentityRole>>().Object,
					new IRoleValidator<IdentityRole>[0],
					new Mock<ILookupNormalizer>().Object,
					new Mock<IdentityErrorDescriber>().Object,
					new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

			var mockIdentityResult = new Mock<IdentityResult>();

			string validRole = "Administrator";
			string validId = "validId";

			var validIdentityResult = new IdentityResult();
			var virType = typeof(IdentityResult);
			PropertyInfo prop = virType.GetProperty("Succeeded", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			prop.SetValue(validIdentityResult, true);

			ApplicationUser validUser = new ApplicationUser();

			mockUserManager.Setup(mock => mock.FindByIdAsync(validId))
				.Returns(Task.FromResult(validUser));

			mockUserManager.Setup(mock => mock.AddToRoleAsync(validUser, validRole))
				.Returns(Task.FromResult(validIdentityResult));

			userServiceMock.Setup(mock => mock.UpdateRole(validUser))
				.Returns(Task.FromResult(validIdentityResult));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			var result = await SUT.Promote(validId);

			// Assert
			Assert.IsInstanceOfType(result, typeof(PartialViewResult));
		}

		[TestMethod]
		public async Task ReturnCorrectViewModel_WhenInvoked()
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

			var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
					new Mock<IRoleStore<IdentityRole>>().Object,
					new IRoleValidator<IdentityRole>[0],
					new Mock<ILookupNormalizer>().Object,
					new Mock<IdentityErrorDescriber>().Object,
					new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

			string validRole = "Administrator";
			string validId = "validId";

			var validIdentityResult = new IdentityResult();
			var virType = typeof(IdentityResult);
			PropertyInfo prop = virType.GetProperty("Succeeded", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			prop.SetValue(validIdentityResult, true);

			ApplicationUser validUser = new ApplicationUser();

			mockUserManager.Setup(mock => mock.FindByIdAsync(validId))
				.Returns(Task.FromResult(validUser));

			mockUserManager.Setup(mock => mock.AddToRoleAsync(validUser, validRole))
				.Returns(Task.FromResult(validIdentityResult));

			userServiceMock.Setup(mock => mock.UpdateRole(validUser))
				.Returns(Task.FromResult(validIdentityResult));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			var result = await SUT.Promote(validId) as PartialViewResult;

			// Assert
			Assert.IsInstanceOfType(result.Model, typeof(UserTableViewModel));
		}

		[TestMethod]
		public async Task CallFindByIdAsync_WhenInvoked()
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

			var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
					new Mock<IRoleStore<IdentityRole>>().Object,
					new IRoleValidator<IdentityRole>[0],
					new Mock<ILookupNormalizer>().Object,
					new Mock<IdentityErrorDescriber>().Object,
					new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

			string validRole = "Administrator";
			string validId = "validId";

			var validIdentityResult = new IdentityResult();
			var virType = typeof(IdentityResult);
			PropertyInfo prop = virType.GetProperty("Succeeded", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			prop.SetValue(validIdentityResult, true);

			ApplicationUser validUser = new ApplicationUser();

			mockUserManager.Setup(mock => mock.FindByIdAsync(validId))
				.Returns(Task.FromResult(validUser));

			mockUserManager.Setup(mock => mock.AddToRoleAsync(validUser, validRole))
				.Returns(Task.FromResult(validIdentityResult));

			userServiceMock.Setup(mock => mock.UpdateRole(validUser))
				.Returns(Task.FromResult(validIdentityResult));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			await SUT.Promote(validId);

			// Assert
			mockUserManager
				.Verify(mock => mock.FindByIdAsync(validId),
				Times.Once);
		}

		[TestMethod]
		public async Task CallAddToRoleAsync_WhenInvoked()
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

			var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
					new Mock<IRoleStore<IdentityRole>>().Object,
					new IRoleValidator<IdentityRole>[0],
					new Mock<ILookupNormalizer>().Object,
					new Mock<IdentityErrorDescriber>().Object,
					new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

			string validRole = "Administrator";
			string validId = "validId";

			var validIdentityResult = new IdentityResult();
			var virType = typeof(IdentityResult);
			PropertyInfo prop = virType.GetProperty("Succeeded", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			prop.SetValue(validIdentityResult, true);

			ApplicationUser validUser = new ApplicationUser();

			mockUserManager.Setup(mock => mock.FindByIdAsync(validId))
				.Returns(Task.FromResult(validUser));

			mockUserManager.Setup(mock => mock.AddToRoleAsync(validUser, validRole))
				.Returns(Task.FromResult(validIdentityResult));

			userServiceMock.Setup(mock => mock.UpdateRole(validUser))
				.Returns(Task.FromResult(validIdentityResult));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			await SUT.Promote(validId);

			// Assert
			mockUserManager
				.Verify(mock => mock.AddToRoleAsync(validUser, validRole),
				Times.Once);
		}

		[TestMethod]
		public async Task CallUpdateRole_WhenInvoked()
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

			var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
					new Mock<IRoleStore<IdentityRole>>().Object,
					new IRoleValidator<IdentityRole>[0],
					new Mock<ILookupNormalizer>().Object,
					new Mock<IdentityErrorDescriber>().Object,
					new Mock<ILogger<RoleManager<IdentityRole>>>().Object);

			string validRole = "Administrator";
			string validId = "validId";

			var validIdentityResult = new IdentityResult();
			var virType = typeof(IdentityResult);
			PropertyInfo prop = virType.GetProperty("Succeeded", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			prop.SetValue(validIdentityResult, true);

			ApplicationUser validUser = new ApplicationUser();

			mockUserManager.Setup(mock => mock.FindByIdAsync(validId))
				.Returns(Task.FromResult(validUser));

			mockUserManager.Setup(mock => mock.AddToRoleAsync(validUser, validRole))
				.Returns(Task.FromResult(validIdentityResult));

			userServiceMock.Setup(mock => mock.UpdateRole(validUser))
				.Returns(Task.FromResult(validIdentityResult));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			await SUT.Promote(validId);

			// Assert
			userServiceMock
				.Verify(mock => mock.UpdateRole(validUser),
				Times.Once);
		}

		[TestMethod]
		public async Task ThrowApplicationexception_WhenPassedNullUser()
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

			var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
					new Mock<IRoleStore<IdentityRole>>().Object,
					new IRoleValidator<IdentityRole>[0],
					new Mock<ILookupNormalizer>().Object,
					new Mock<IdentityErrorDescriber>().Object,
					new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
			
			string validId = "validId";

			ApplicationUser nullUser = null;

			mockUserManager.Setup(mock => mock.FindByIdAsync(validId))
				.Returns(Task.FromResult(nullUser));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			//Act & Assert
			await Assert.ThrowsExceptionAsync<ApplicationException>(() =>
				SUT.Promote(validId));
		}

		[TestMethod]
		public async Task ThrowApplicationexception_WhenPassedNullId()
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

			var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
					new Mock<IRoleStore<IdentityRole>>().Object,
					new IRoleValidator<IdentityRole>[0],
					new Mock<ILookupNormalizer>().Object,
					new Mock<IdentityErrorDescriber>().Object,
					new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
			
			string nullId = null;

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act & Assert
			await Assert.ThrowsExceptionAsync<ApplicationException>(() =>
				SUT.Promote(nullId));
		}
	}
}
