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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace SD.Web.Tests.Areas.Administration.UserControllerTests
{
	[TestClass]
	public class IndexAction_Should
	{
		[TestMethod]
		public async Task ReturnViewResult_WhenCalled()
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

			string validFilter = string.Empty;
			int validPageNumber = 1;
			int validPageSize = 10;

			IPagedList<ApplicationUser> users = new PagedList<ApplicationUser>(new List<ApplicationUser>().AsQueryable(), validPageNumber, validPageSize);

			userServiceMock.Setup(mock => mock.FilterUsersAsync(validFilter, validPageNumber, validPageSize))
				.Returns(Task.FromResult(users));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			var result = await SUT.Index();

			// Assert
			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}

		[TestMethod]
		public async Task ReturnCorrectViewModel_WhenCalled()
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

			string validFilter = string.Empty;
			int validPageNumber = 1;
			int validPageSize = 10;

			IPagedList<ApplicationUser> users = new PagedList<ApplicationUser>(new List<ApplicationUser>().AsQueryable(), validPageNumber, validPageSize);
			
			userServiceMock.Setup(mock => mock.FilterUsersAsync(validFilter, validPageNumber, validPageSize))
				.Returns(Task.FromResult(users));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			var result = await SUT.Index() as ViewResult;

			// Assert
			Assert.IsInstanceOfType(result.Model, typeof(UserIndexViewModel));
		}

		[TestMethod]
		public async Task CallFilterUsersAsync_WhenInvoked()
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

			string validFilter = string.Empty;
			int validPageNumber = 1;
			int validPageSize = 10;

			IPagedList<ApplicationUser> users = new PagedList<ApplicationUser>(new List<ApplicationUser>().AsQueryable(), validPageNumber, validPageSize);

			userServiceMock.Setup(mock => mock.FilterUsersAsync(validFilter, validPageNumber, validPageSize))
				.Returns(Task.FromResult(users));

			UserController SUT = new UserController(
					 userServiceMock.Object,
					 mockRoleManager.Object,
					 mockUserManager.Object);

			// Act
			await SUT.Index();

			// Assert
			userServiceMock
				.Verify(mock => mock.FilterUsersAsync(validFilter, validPageNumber, validPageSize),
				Times.Once);
		}
	}
}
