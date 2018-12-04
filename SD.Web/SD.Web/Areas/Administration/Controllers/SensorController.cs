using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SD.Data.Models.DomainModels;
using SD.Services.Data.Services.Contracts;
using SD.Web.Areas.Administration.Models;
using SD.Web.Areas.Administration.Models.SensorViewModels;
using SD.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace SD.Web.Areas.Administration.Controllers
{
	[Area("Administration")]
	[Authorize(Policy = "Admin")]
	public class SensorController : Controller
	{
		private readonly IUserSensorService _userSensorService;
		private readonly ISensorService _sensorService;
		private readonly IMemoryCache _memoryCache;
        private readonly ISensorDataService _sensorDataService;

        public SensorController(IUserSensorService userSensorService, ISensorService sensorService, IMemoryCache memoryCache, ISensorDataService sensorDataService)
		{
			_userSensorService = userSensorService ?? throw new ArgumentNullException(nameof(userSensorService));
			_sensorService = sensorService ?? throw new ArgumentNullException(nameof(sensorService));
			_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _sensorDataService = sensorDataService ?? throw new ArgumentNullException(nameof(sensorDataService));
		}

		[HttpGet("usersensors")]
		public async Task<IActionResult> Index()
		{
			if (!_memoryCache.TryGetValue("ListOfUserSensors", out IPagedList<UserSensor> userSensors))
			{
				userSensors = await _userSensorService.FilterUserSensorsAsync();

				MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(25),
					SlidingExpiration = TimeSpan.FromSeconds(5)
				};

				_memoryCache.Set("ListOfUserSensors", userSensors, options);
			}

			var model = new SensorIndexViewModel(userSensors);

			return View(model);
		}

		[HttpGet("usersensors/{id}")]
		public async Task<IActionResult> Index(string id, int? pageNumber, int? pageSize)
		{
			var userSensors = await _userSensorService.GetSensorsByUserId(id, pageNumber ?? 1, pageSize ?? 10);

			var model = new SensorIndexViewModel(userSensors, string.Empty);

			return View(model);
		}

		[HttpGet("usersensors/filter")]
		public async Task<IActionResult> Filter(string searchTerm, int? pageSize, int? pageNumber)
		{
			searchTerm = searchTerm ?? string.Empty;

			var userSensors = await _userSensorService.FilterUserSensorsAsync(searchTerm, pageNumber ?? 1, pageSize ?? 10);

			var model = new SensorIndexViewModel(userSensors, searchTerm);

			return PartialView("_SensorTablePartial", model.Table);
		}
		
		[HttpGet("regusersensor/{userId}")]
		public async Task<IActionResult> Register(string userId, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			var allSensorIds = await _sensorService.GetSensorNamesIdsAsync();
			var model = new RegisterViewModel(userId, allSensorIds);
			return View(model);
		}

		[HttpPost("regusersensor/{userId}")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;

            var sensor = await _sensorDataService.GetSensorsByIdAsync(model.SensorId);
            model.LastValueUser = sensor.SensorData.Last(s => s.SensorId == model.SensorId).Value;

            if (ModelState.IsValid)
			{
				var userSensor = await _userSensorService.AddUserSensorAsync(model.UserId, model.SensorId, model.Name, model.Description, 
					model.Latitude, model.Longitude, model.AlarmMin, model.AlarmMax, model.PollingInterval, model.AlarmTriggered, model.IsPublic, model.LastValueUser, model.Type);
				return RedirectToLocal(returnUrl);
			}

			return View(model);
		}

		[HttpGet("usersensors/modify/{id}")]
		public async Task<IActionResult> Modify(string id)
		{
			var userSensor = await _userSensorService.GetSensorByIdAsync(id);

			//handle null user sensor

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

			var userSensor = await _userSensorService.GetSensorByIdAsync(model.Id.ToString());

			userSensor.Name = model.Name;
			userSensor.Description = model.Description;
			userSensor.AlarmTriggered = model.AlarmTriggered;
			userSensor.AlarmMin = model.AlarmMin;
			userSensor.AlarmMax = model.AlarmMax;
			userSensor.IsPublic = model.IsPublic;
			userSensor.PollingInterval = model.PollingInterval;
			userSensor.Latitude = model.Latitude;
			userSensor.Longitude = model.Longitude;

			await _userSensorService.UpdateUserSensorAsync(userSensor);
			
			return RedirectToAction(nameof(Modify));
		}

		[NonAction]
		private IActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction(nameof(HomeController.Index), "Home");
			}
		}
	}
}
