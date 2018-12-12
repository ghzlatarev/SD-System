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
using System.Reflection;
using System.Threading.Tasks;

namespace SD.Web.Tests.Areas.Administration.UserControllerTests
{
	[TestClass]
	public class DemoteAction_Should
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

			mockRoleManager.Setup(mock => mock.RoleExistsAsync(validRole))
				.Returns(Task.FromResult(true));

			mockUserManager.Setup(mock => mock.FindByIdAsync(validId))
				.Returns(Task.FromResult(validUser));

			mockUserManager.Setup(mock => mock.RemoveFromRoleAsync(validUser, validRole))
				.Returns(Task.FromResult(validIdentityResult));

			userServiceMock.Setup(mock => mock.UpdateRole(validUser))
				.Returns(Task.FromResult(validIdentityResult));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			var result = await SUT.Demote(validId);

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

			mockRoleManager.Setup(mock => mock.RoleExistsAsync(validRole))
				.Returns(Task.FromResult(true));

			mockUserManager.Setup(mock => mock.FindByIdAsync(validId))
				.Returns(Task.FromResult(validUser));

			mockUserManager.Setup(mock => mock.RemoveFromRoleAsync(validUser, validRole))
				.Returns(Task.FromResult(validIdentityResult));

			userServiceMock.Setup(mock => mock.UpdateRole(validUser))
				.Returns(Task.FromResult(validIdentityResult));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			var result = await SUT.Demote(validId) as PartialViewResult;

			// Assert
			Assert.IsInstanceOfType(result.Model, typeof(UserTableViewModel));
		}

		[TestMethod]
		public async Task CallRoleExistsAsync_WhenInvoked()
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

			mockRoleManager.Setup(mock => mock.RoleExistsAsync(validRole))
				.Returns(Task.FromResult(true));

			mockUserManager.Setup(mock => mock.FindByIdAsync(validId))
				.Returns(Task.FromResult(validUser));

			mockUserManager.Setup(mock => mock.RemoveFromRoleAsync(validUser, validRole))
				.Returns(Task.FromResult(validIdentityResult));

			userServiceMock.Setup(mock => mock.UpdateRole(validUser))
				.Returns(Task.FromResult(validIdentityResult));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			await SUT.Demote(validId);

			// Assert
			mockRoleManager
				.Verify(mock => mock.RoleExistsAsync(validRole),
				Times.Once);
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

			mockRoleManager.Setup(mock => mock.RoleExistsAsync(validRole))
				.Returns(Task.FromResult(true));

			mockUserManager.Setup(mock => mock.FindByIdAsync(validId))
				.Returns(Task.FromResult(validUser));

			mockUserManager.Setup(mock => mock.RemoveFromRoleAsync(validUser, validRole))
				.Returns(Task.FromResult(validIdentityResult));

			userServiceMock.Setup(mock => mock.UpdateRole(validUser))
				.Returns(Task.FromResult(validIdentityResult));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			await SUT.Demote(validId);

			// Assert
			mockUserManager
				.Verify(mock => mock.FindByIdAsync(validId),
				Times.Once);
		}

		[TestMethod]
		public async Task CallRemoveFromRoleAsync_WhenInvoked()
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

			mockRoleManager.Setup(mock => mock.RoleExistsAsync(validRole))
				.Returns(Task.FromResult(true));

			mockUserManager.Setup(mock => mock.FindByIdAsync(validId))
				.Returns(Task.FromResult(validUser));

			mockUserManager.Setup(mock => mock.RemoveFromRoleAsync(validUser, validRole))
				.Returns(Task.FromResult(validIdentityResult));

			userServiceMock.Setup(mock => mock.UpdateRole(validUser))
				.Returns(Task.FromResult(validIdentityResult));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			await SUT.Demote(validId);

			// Assert
			mockUserManager
				.Verify(mock => mock.RemoveFromRoleAsync(validUser, validRole),
				Times.Once);
		}

		[TestMethod]
		public async Task CallRemoveUpdateRole_WhenInvoked()
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

			mockRoleManager.Setup(mock => mock.RoleExistsAsync(validRole))
				.Returns(Task.FromResult(true));

			mockUserManager.Setup(mock => mock.FindByIdAsync(validId))
				.Returns(Task.FromResult(validUser));

			mockUserManager.Setup(mock => mock.RemoveFromRoleAsync(validUser, validRole))
				.Returns(Task.FromResult(validIdentityResult));

			userServiceMock.Setup(mock => mock.UpdateRole(validUser))
				.Returns(Task.FromResult(validIdentityResult));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			await SUT.Demote(validId);

			// Assert
			userServiceMock
				.Verify(mock => mock.UpdateRole(validUser),
				Times.Once);
		}

		[TestMethod]
		public async Task ThrowApplicationException_WhenRoleDoesNotExist()
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

			string invalidRole = "invalidRole";
			string validId = "validId";

			mockRoleManager.Setup(mock => mock.RoleExistsAsync(invalidRole))
				.Returns(Task.FromResult(false));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act & Assert
			await Assert.ThrowsExceptionAsync<ApplicationException>(() =>
				SUT.Demote(validId));
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

			string validRole = "validRole";
			string validId = "validId";

			ApplicationUser nullUser = null;

			mockRoleManager.Setup(mock => mock.RoleExistsAsync(validRole))
				.Returns(Task.FromResult(true));

			mockUserManager.Setup(mock => mock.FindByIdAsync(validId))
				.Returns(Task.FromResult(nullUser));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);
			
			//Act & Assert
			await Assert.ThrowsExceptionAsync<ApplicationException>(() =>
				SUT.Demote(validId));
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

			string validRole = "validRole";
			string nullId = null;

			mockRoleManager.Setup(mock => mock.RoleExistsAsync(validRole))
				.Returns(Task.FromResult(true));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act & Assert
			await Assert.ThrowsExceptionAsync<ApplicationException>(() =>
				SUT.Demote(nullId));
		}
	}
}
