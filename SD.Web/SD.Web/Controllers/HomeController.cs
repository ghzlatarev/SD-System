using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SD.Services.Data.Services;
using SD.Services.Data.Services.Contracts;
using SD.Web.Models;

namespace SD.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISensorDataService service;

        public HomeController(ISensorDataService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            //await this.service.GetSensorsData();
            return View();
        }

        //private readonly ISensorService service;

        //public HomeController(ISensorService service)
        //{
        //    this.service = service;
        //}

        //public async Task<IActionResult> Index()
        //{
        //    await this.service.RebaseSensorsAsync();
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
