using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using SD.Services.Data.Services;
using SD.Services.Data.Services.Contracts;
using SD.Web.Models;
using SD.Web.Areas.UserRegular.Models;
using SD.Web.Areas.UserRegular.Controllers;
using Kendo.Mvc.UI;
using SD.Data.Context;
using Kendo.Mvc.Extensions;
using Microsoft.EntityFrameworkCore;
using SD.Data.Models.DomainModels;

namespace SD.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ISensorDataService _service;
		private readonly IUserSensorService _userSensorService;
		private readonly ISensorService _serviceSensor;
		private readonly DataContext _dataContext;

		public HomeController(ISensorDataService service, IUserSensorService userSensorService, ISensorService serviceSensor, DataContext dataContext)
		{
			_service = service;
			_userSensorService = userSensorService;
			_serviceSensor = serviceSensor;
			_dataContext = dataContext;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		public async Task<JsonResult> Get_Sensors([DataSourceRequest] DataSourceRequest request)
		{
			var sensors = await _userSensorService.ListPublicSensorsAsync();

			var result = sensors.Select(s => new UserSensorViewModel
			{
				Name = s.Name,
				Description = s.Description,
				Type = s.Type,
				UserInterval = s.UserInterval,
				LastValueUser = s.LastValueUser,
				TimeStamp = s.TimeStamp,
				Coordinates = s.Coordinates,
				IsPublic = s.IsPublic,
				AlarmTriggered = s.AlarmTriggered,
				AlarmMin = s.AlarmMin,
				AlarmMax = s.AlarmMax,
				UserId = s.UserId,
				SensorId = s.SensorId,
				Id = s.Id
			});

			return this.Json(result.ToDataSourceResult(request));
		}


		//public async Task<IActionResult> Index()
		//{
		//    await _serviceSensor.RebaseSensorsAsync();
		//    return View();
		//}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public double test()
		{
			Random rnd = new Random();
			double value = rnd.Next(10, 100);
			return value;
		}
	}
}
