﻿using AspMvcUdemyPractice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspMvcUdemyPractice.Areas.Customer.Controllers
{
    [Area("Customer")] // Tells the controller that this is belong to a specific area
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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