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

namespace SD.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISensorDataService _service;
        private readonly ISensorService _sensorService;

        public HomeController(ISensorDataService service, ISensorService sensorService)
        {
            _service = service;
            _sensorService = sensorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            var sensors = await _sensorService.ListPublicSensorsAsync();

            var model = new UserSensorsViewModel()
            {
                userSensorViewModels = sensors.Select(se => new UserSensorViewModel(se))
            };

            return View(model);
        }

       

        //private readonly ISensorService service;

        //public HomeController(ISensorService service)
        //{
        //    this._service = service;
        //}

        //public async Task<IActionResult> Index()
        //{
        //    await this._service.RebaseSensorsAsync();
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
    }
}
