using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services.Contracts;
using SD.Web.Areas.Administration.Models.SensorViewModels;
using SD.Web.Controllers;
using System;
using System.Threading.Tasks;
using X.PagedList;

namespace SD.Web.Areas.Administration.Controllers
{
	[Area("Administration")]
	[Authorize(Policy = "Admin")]
	public class SensorController : Controller
	{
		private readonly IUserSensorService userSensorService;
		private readonly ISensorService sensorService;
		private readonly IMemoryCache memoryCache;
        private readonly ISensorDataService sensorDataService;

        public SensorController(IUserSensorService userSensorService, ISensorService sensorService, IMemoryCache memoryCache, ISensorDataService sensorDataService)
		{
			this.userSensorService = userSensorService ?? throw new ArgumentNullException(nameof(userSensorService));
			this.sensorService = sensorService ?? throw new ArgumentNullException(nameof(sensorService));
			this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
			this.sensorDataService = sensorDataService ?? throw new ArgumentNullException(nameof(sensorDataService));
		}

		[HttpGet("usersensors")]
		public async Task<IActionResult> Index()
		{
			if (!this.memoryCache.TryGetValue("ListOfUserSensors", out IPagedList<UserSensor> userSensors))
			{
				userSensors = await this.userSensorService.FilterUserSensorsAsync();

				MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(25),
					SlidingExpiration = TimeSpan.FromSeconds(5)
				};

				this.memoryCache.Set("ListOfUserSensors", userSensors, options);
			}

			var model = new SensorIndexViewModel(userSensors);

			return View(model);
		}

		[HttpGet("usersensors/{id}")]
		public async Task<IActionResult> Index(string id, int? pageNumber, int? pageSize)
		{
			if (id == null)
			{
				throw new ApplicationException($"Passed ID parameter is absent.");
			}

			var userSensors = await this.userSensorService.GetSensorsByUserId(id, pageNumber ?? 1, pageSize ?? 10);

			if (userSensors.Count == 0)
			{
				throw new ApplicationException($"Unable to find user sensors for user with ID '{id}'.");
			}

			var model = new SensorIndexViewModel(userSensors, string.Empty);

			return View(model);
		}

		[HttpGet("usersensors/filter")]
		public async Task<IActionResult> Filter(string searchTerm, int? pageSize, int? pageNumber)
		{
			searchTerm = searchTerm ?? string.Empty;

			var userSensors = await this.userSensorService.FilterUserSensorsAsync(searchTerm, pageNumber ?? 1, pageSize ?? 10);

			var model = new SensorIndexViewModel(userSensors, searchTerm);

			return PartialView("_SensorTablePartial", model.Table);
		}
		
		[HttpGet("regusersensor/{userId}")]
		public async Task<IActionResult> Register(string userId, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			var stateSensors = await this.sensorService.ListStateSensorsAsync();
			var nonStateSensors = await this.sensorService.ListNonStateSensorsAsync();
			var model = new RegisterViewModel(userId, stateSensors, nonStateSensors);
			return View(model);
		}

		[HttpPost("regusersensor/{userId}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			
            if (ModelState.IsValid)
			{
				var sensor = await this.sensorService.GetSensorByIdAsync(model.SensorId);
				model.LastValueUser = sensor.LastValue;
				model.IsState = sensor.IsState;
				model.Type = sensor.MeasureType;

				var userSensor = await this.userSensorService.AddUserSensorAsync(model.UserId, model.SensorId, 
					model.Name, model.Description, model.Latitude.ToString(), model.Longitude.ToString(),
					model.AlarmMin, model.AlarmMax, model.PollingInterval, model.AlarmTriggered, model.IsPublic, 
					model.LastValueUser, model.Type);

				return RedirectToAction(nameof(HomeController.Index), "Home");
			}

			return View(model);
		}

		[HttpGet("usersensors/modify/{id}")]
		public async Task<IActionResult> Modify(string id)
		{
			if (id == null)
			{
				throw new ApplicationException($"Passed ID parameter is absent.");
			}

			var userSensor = await this.userSensorService.GetSensorByIdAsync(id);

			if (userSensor == null)
			{
				throw new ApplicationException($"Unable to find user sensor with ID '{id}'.");
			}

			var model = new ModifyViewModel(userSensor);

			return View(model);
		}

		[HttpPost("usersensors/modify/{id}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Modify(ModifyViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var userSensor = await this.userSensorService.GetSensorByIdAsync(model.Id.ToString());

			userSensor.Name = model.Name;
			userSensor.Description = model.Description;
			userSensor.AlarmTriggered = model.AlarmTriggered;
			userSensor.AlarmMin = model.AlarmMin;
			userSensor.AlarmMax = model.AlarmMax;
			userSensor.IsPublic = model.IsPublic;
			userSensor.PollingInterval = model.PollingInterval;
			userSensor.Latitude = model.Latitude.ToString();
			userSensor.Longitude = model.Longitude.ToString();
			userSensor.Coordinates = model.Latitude + "," + model.Longitude;

			await this.userSensorService.UpdateUserSensorAsync(userSensor);
			
			return RedirectToAction(nameof(Modify));
		}
	}
}
