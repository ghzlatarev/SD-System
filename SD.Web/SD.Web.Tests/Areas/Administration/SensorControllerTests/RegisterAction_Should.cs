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
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SD.Web.Tests.Areas.Administration.SensorControllerTests
{
	[TestClass]
	public class RegisterAction_Should
	{
		[TestMethod]
		public async Task CallListStateSensorsAsync_WhenInvoked()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validUserId = Guid.NewGuid().ToString();

			IEnumerable<Sensor> stateSensors = new List<Sensor>();
			IEnumerable<Sensor> nonStateSensors = new List<Sensor>();

			sensorServiceMock.Setup(mock => mock.ListStateSensorsAsync())
				.Returns(Task.FromResult(stateSensors));

			sensorServiceMock.Setup(mock => mock.ListStateSensorsAsync())
				.Returns(Task.FromResult(nonStateSensors));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act
			await SUT.Register(validUserId);

			// Assert
			sensorServiceMock.Verify(mock => mock.ListStateSensorsAsync(), Times.Once);
		}

		[TestMethod]
		public async Task CallLisNontStateSensorsAsync_WhenInvoked()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validUserId = Guid.NewGuid().ToString();

			IEnumerable<Sensor> stateSensors = new List<Sensor>();
			IEnumerable<Sensor> nonStateSensors = new List<Sensor>();

			sensorServiceMock.Setup(mock => mock.ListStateSensorsAsync())
				.Returns(Task.FromResult(stateSensors));

			sensorServiceMock.Setup(mock => mock.ListStateSensorsAsync())
				.Returns(Task.FromResult(nonStateSensors));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act
			await SUT.Register(validUserId);

			// Assert
			sensorServiceMock.Verify(mock => mock.ListNonStateSensorsAsync(), Times.Once);
		}

		[TestMethod]
		public async Task ReturnViewResult_WhenInvoked()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validUserId = Guid.NewGuid().ToString();

			IEnumerable<Sensor> stateSensors = new List<Sensor>();
			IEnumerable<Sensor> nonStateSensors = new List<Sensor>();

			sensorServiceMock.Setup(mock => mock.ListStateSensorsAsync())
				.Returns(Task.FromResult(stateSensors));

			sensorServiceMock.Setup(mock => mock.ListStateSensorsAsync())
				.Returns(Task.FromResult(nonStateSensors));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act
			var result = await SUT.Register(validUserId);

			// Assert
			Assert.IsInstanceOfType(result, typeof(ViewResult));
		}

		[TestMethod]
		public async Task ReturnCorrectViewModel_WhenInvoked()
		{
			// Arrange
			Mock<IUserSensorService> userSensorServiceMock = new Mock<IUserSensorService>();
			Mock<ISensorService> sensorServiceMock = new Mock<ISensorService>();
			Mock<IMemoryCache> memoryCacheMock = new Mock<IMemoryCache>();
			Mock<ISensorDataService> sensorDataServiceMock = new Mock<ISensorDataService>();

			string validUserId = Guid.NewGuid().ToString();

			IEnumerable<Sensor> stateSensors = new List<Sensor>();
			IEnumerable<Sensor> nonStateSensors = new List<Sensor>();

			sensorServiceMock.Setup(mock => mock.ListStateSensorsAsync())
				.Returns(Task.FromResult(stateSensors));

			sensorServiceMock.Setup(mock => mock.ListStateSensorsAsync())
				.Returns(Task.FromResult(nonStateSensors));

			SensorController SUT = new SensorController(
				userSensorServiceMock.Object,
				sensorServiceMock.Object,
				memoryCacheMock.Object,
				sensorDataServiceMock.Object);

			// Act
			var result = await SUT.Register(validUserId) as ViewResult;

			// Assert
			Assert.IsInstanceOfType(result.Model, typeof(RegisterViewModel));
		}

		[TestMethod]
		public void ReturnLocalRedirectResult_WhenModelStateIsValid()
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

			var viewModel = new RegisterViewModel()
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
				UserId = "UserId",
				SensorId = "SensorId",
				IsState = true,
			};

			UserSensor validUserSensorResult = new UserSensor();
			Sensor validSensor = new Sensor
			{
				IsState = false
			};
			validUserSensorResult.Sensor = validSensor;
			validUserSensorResult.Latitude = "0";
			validUserSensorResult.Longitude = "0";

			sensorServiceMock.Setup(mock => mock.GetSensorByIdAsync(viewModel.SensorId))
				.Returns(Task.FromResult(validSensor));

			userSensorServiceMock.Setup(mock => mock.AddUserSensorAsync(viewModel.UserId,
				viewModel.SensorId, viewModel.Name, viewModel.Description, viewModel.Latitude.ToString(),
				viewModel.Longitude.ToString(), viewModel.AlarmMin, viewModel.AlarmMax, viewModel.PollingInterval,
				viewModel.AlarmTriggered, viewModel.IsPublic, viewModel.LastValueUser, viewModel.Type))
				.Returns(Task.FromResult(validUserSensorResult));

			// Act
			var result = controller.Register(viewModel);

			//Assert
			Assert.IsInstanceOfType(result.Result, typeof(RedirectToActionResult));
			var redirectResult = (RedirectToActionResult)result.Result;
			Assert.AreEqual("Home", redirectResult.ControllerName);
			Assert.AreEqual("Index", redirectResult.ActionName);
			Assert.IsNull(redirectResult.RouteValues);
		}

		[TestMethod]
		public void RedisplayView_WhenPassedInvalidModelState()
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

			controller.ModelState.AddModelError("error", "error");
			var viewModel = new RegisterViewModel();

			// Act
			var result = controller.Register(viewModel);

			// Assert
			Assert.IsInstanceOfType(result.Result, typeof(ViewResult));
			var viewResult = (ViewResult)result.Result;
			Assert.IsInstanceOfType(viewResult.Model, typeof(RegisterViewModel));
			Assert.IsNull(viewResult.ViewName); // should not return any other view
		}
	}
}
