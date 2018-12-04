﻿using System;
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

namespace SD.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISensorDataService _service;
        private readonly IUserSensorService _userSensorService;
        private readonly ISensorService _serviceSensor;

        public HomeController(ISensorDataService service, IUserSensorService userSensorService, ISensorService serviceSensor)
        {
            _service = service;
            _userSensorService = userSensorService;
            _serviceSensor = serviceSensor;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            var sensors = await _userSensorService.ListPublicSensorsAsync();

            var model = new UserSensorsViewModel()
            {
                userSensorViewModels = sensors.Select(se => new UserSensorViewModel(se))
            };

            return View(model);
        }


        public async Task<IActionResult> Index()
        {
            await _serviceSensor.RebaseSensorsAsync();
            return View();
        }

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
    }
}
