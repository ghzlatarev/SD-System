using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SD.Data.Models.Identity;
using SD.Services.Data.Services.Contracts;
using SD.Services.Data.Services.Identity.Contracts;
using SD.Web.Areas.UserRegular.Models;

namespace SD.Web.Areas.UserRegular.Controllers
{
    [Area("UserRegular")]
    public class DashboardController : Controller
    {
        private readonly IUserSensorService _userSensorService;
        private readonly ISensorDataService _sensorDataService;
        private readonly ISensorService _sensorService;
        private readonly UserManager<ApplicationUser> _userManager;
 
        public DashboardController(IUserSensorService userSensorService, UserManager<ApplicationUser> userManager, ISensorService sensorService, ISensorDataService sensorDataService)
        {
            _userSensorService = userSensorService;
            _userManager = userManager;
            _sensorService = sensorService;
            _sensorDataService = sensorDataService;
    }

        [HttpGet("list-sensors")]
        public async Task<IActionResult> Index(Guid id)
        {
            var user = HttpContext.User;
            var userId = _userManager.GetUserId(user);
            var sensors = await _userSensorService.ListSensorsForUserAsync(userId);

            var model = new UserSensorsViewModel()
            {
                userSensorViewModels = sensors.Select(se => new UserSensorViewModel(se))
            };

            return View(model);
        }

        [HttpPost("list-sensors")]
        [ValidateAntiForgeryToken]
        public IActionResult Index(UserSensorViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                //return RedirectToAction("ListSensor", "dashboard", new { id = model.Id });
                RedirectToAction(nameof(DashboardController.ListSensor), new { id = model.Id });
            }
            return View(model);
        }

        [HttpGet("sensor")]
        public async Task<IActionResult> ListSensor(string id)
        {
            var sensor = await _userSensorService.ListSensorByIdAsync(id);
            
            var model = new UserSensorViewModel(sensor)
            {
                Name = sensor.Name,
                Description = sensor.Description,
                Type = sensor.Type,
                UserInterval = sensor.UserInterval,
                LastValueUser = sensor.LastValueUser,
                TimeStamp = sensor.TimeStamp,
                Coordinates = sensor.Coordinates,
                IsPublic = sensor.IsPublic,
                AlarmTriggered = sensor.AlarmTriggered,
                AlarmMin = sensor.AlarmMin,
                AlarmMax = sensor.AlarmMax,
                UserId = sensor.UserId,
                User = sensor.User,
                SensorId = sensor.SensorId,
                Sensor = sensor.Sensor,
                Id = sensor.Id                
            };

            return View(model);
        }


        [HttpGet("choose-data-source")]
        public async Task<IActionResult> ChooseDataSource(string id)
        {
            var sensors = await _sensorService.ListSensorsAsync();


            var model = new DataSourceViewModel()
            {
                SensorApi = sensors.Select(se => new SensorAPIViewModel(se))
            };

            return View(model);
        }

        [HttpPost("choose-data-source")]
        [ValidateAntiForgeryToken]
        public ActionResult ChooseDataSource(DataSourceViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                //var userId = int.Parse(this.userWrapper.GetUserId(this.User));
                //var sensor = await sensorDataService.

                return RedirectToAction("register", "dashboard", new { id = model.SensorApi.Select(s => s.SensorId) });
            }

            return this.View(model);
        }

        [HttpGet("register-sensor")]
        public async Task<IActionResult> Register(string id)
        {
            var sensor = await _sensorDataService.GetSensorsByIdAsync(id);
            var user = HttpContext.User;
            var userId = _userManager.GetUserId(user);

            var model = new SensorRegistrationByUserModel()
            {
                Description = sensor.Description,
                SensorId = sensor.SensorId,
                Type = sensor.MeasureType,
                Tag = sensor.Tag,
                ApiInterval = sensor.MinPollingIntervalInSeconds,
                UserId = userId,
                Id = sensor.Id
            };

            return View(model);
        }

        [HttpPost("register-sensor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(SensorRegistrationByUserModel model)
        {
            var sensor = await _sensorDataService.GetSensorsByIdAsync(model.SensorId);
            model.LastValueUser = sensor.SensorData.Last(s => s.SensorId == model.SensorId).Value;

            if (this.ModelState.IsValid)
            {
                //TODO: not passing lastvalue and type
                
                await this._userSensorService.AddUserSensorAsync(model.UserId, model.SensorId, model.Name, model.UserDescription, model.Coordinates.Split(',')[0],
                    model.Coordinates.Split(',')[1], model.AlarmMin, model.AlarmMax, model.UserInterval, model.AlarmTriggered, model.IsPublic,
                    model.LastValueUser, model.Type);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("update-sensor")]

        public async Task<IActionResult> UpdateSensor(SensorRegistrationByUserModel model)
        {
            var sensor = await _userSensorService.GetSensorByIdAsync(model.SensorId);

            sensor.Name = model.Name;
            sensor.Description = model.Description;

            if (this.ModelState.IsValid)
            {
                //TODO: not passing lastvalue and type

                await this._userSensorService.UpdateUserSensorAsync(sensor);
            }

            return Json(sensor);
        }


    }
}