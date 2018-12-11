using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services.Contracts;
using SD.Web.Areas.Administration.Controllers;
using SD.Web.Areas.Administration.Models.SensorViewModels;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SD.Web.Tests.Areas.Administration.SensorControllerTests
{
	[TestClass]
	public class ModifyAction_Should
	{
		[TestMethod]
		public async Task ReturnViewResult_WhenInvoked()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validId = string.Empty;

			UserSensor validUserSensorResult = new UserSensor();
			Sensor validSensor = new Sensor
			{
				IsState = false
			};
			validUserSensorResult.Sensor = validSensor;
			validUserSensorResult.Latitude = "0";
			validUserSensorResult.Longitude = "0";

			userSensorServiceMock.Setup(mock => mock.GetSensorByIdAsync(validId))
				.Returns(Task.FromResult(validUserSensorResult));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act
			var result = await SUT.Modify(validId);

			// Assert
			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}

		[TestMethod]
		public async Task ReturnCorrectViewModel_WhenCalled()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validId = string.Empty;

			UserSensor validUserSensorResult = new UserSensor();
			Sensor validSensor = new Sensor
			{
				IsState = false
			};
			validUserSensorResult.Sensor = validSensor;
			validUserSensorResult.Latitude = "0";
			validUserSensorResult.Longitude = "0";

			userSensorServiceMock.Setup(mock => mock.GetSensorByIdAsync(validId))
				.Returns(Task.FromResult(validUserSensorResult));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act
			var result = await SUT.Modify(validId) as ViewResult;

			// Assert
			Assert.IsInstanceOfType(result.Model, typeof(ModifyViewModel));
		}

		[TestMethod]
		public async Task CallGetSensorByIdAsync_WhenInvoked()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validId = string.Empty;

			UserSensor validUserSensorResult = new UserSensor();
			Sensor validSensor = new Sensor
			{
				IsState = false
			};
			validUserSensorResult.Sensor = validSensor;
			validUserSensorResult.Latitude = "0";
			validUserSensorResult.Longitude = "0";

			userSensorServiceMock.Setup(mock => mock.GetSensorByIdAsync(validId))
				.Returns(Task.FromResult(validUserSensorResult));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act
			await SUT.Modify(validId);

			// Assert
			userSensorServiceMock.Verify(mock => mock.GetSensorByIdAsync(validId), Times.Once);
		}

		[TestMethod]
		public async Task ThrowApplicationException_WhenPassedNullId()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string nullId = null;

			UserSensor validUserSensor = new UserSensor();
			
			userSensorServiceMock.Setup(mock => mock.GetSensorByIdAsync(nullId))
				.Returns(Task.FromResult(validUserSensor));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act & Assert
			await Assert.ThrowsExceptionAsync<ApplicationException>(() =>
				SUT.Modify(nullId));
		}

		[TestMethod]
		public async Task ThrowApplicationException_WhenPassedUserSensorIsNull()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validId = string.Empty;

			UserSensor validUserSensorResult = null;

			userSensorServiceMock.Setup(mock => mock.GetSensorByIdAsync(validId))
				.Returns(Task.FromResult(validUserSensorResult));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act & Assert
			await Assert.ThrowsExceptionAsync<ApplicationException>(() =>
				SUT.Modify(validId));
		}

		[TestMethod]
		public void ReturnRedirectResult_WhenModelStateIsValid()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			var controller = new SensorController(userSensorServiceMock.Object,
				sensorServiceMock.Object, memoryCacheMock.Object, sensorDataServiceMock.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext()
					{
						User = new ClaimsPrincipal()
					}
				},
				TempData = new Mock<ITempDataDictionary>().Object
			};
			var viewModel = new ModifyViewModel()
			{
				Name = "Name",
				Description = "Description",
				PollingInterval = 10,
				Latitude = 0,
				Longitude = 0,
				IsPublic = true,
				AlarmTriggered = true,
				AlarmMin = 0,
				AlarmMax = 0,
				Id = "Description",
				IsState = true,
			};

			string validId = viewModel.Id;

			UserSensor validUserSensorResult = new UserSensor();
			Sensor validSensor = new Sensor
			{
				IsState = false
			};
			validUserSensorResult.Sensor = validSensor;
			validUserSensorResult.Latitude = "0";
			validUserSensorResult.Longitude = "0";

			userSensorServiceMock.Setup(mock => mock.GetSensorByIdAsync(validId))
				.Returns(Task.FromResult(validUserSensorResult));

			// Act
			var result = controller.Modify(viewModel);

			//Assert
			Assert.IsInstanceOfType(result.Result, typeof(RedirectToActionResult));
			var redirectResult = (RedirectToActionResult)result.Result;
			Assert.AreEqual("Modify", redirectResult.ActionName);
			Assert.IsNull(redirectResult.RouteValues);
		}

		[TestMethod]
		public void RedisplayView_WhenPassedInvalidModelState()
		{
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			var controller = new SensorController(userSensorServiceMock.Object, 
				sensorServiceMock.Object, memoryCacheMock.Object, sensorDataServiceMock.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext()
					{
						User = new ClaimsPrincipal()
					}
				},
				TempData = new Mock<ITempDataDictionary>().Object
			};

			controller.ModelState.AddModelError("error", "error");
			var viewModel = new ModifyViewModel();

			var result = controller.Modify(viewModel);

			Assert.IsInstanceOfType(result.Result, typeof(ViewResult));
			var viewResult = (ViewResult)result.Result;
			Assert.IsInstanceOfType(viewResult.Model, typeof(ModifyViewModel));
			Assert.IsNull(viewResult.ViewName); // should not return any other view
		}
	}
}
