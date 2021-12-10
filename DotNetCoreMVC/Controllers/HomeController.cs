using DotNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DotNetCoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly List<Laptop> _laptops = new()
        {
            new Laptop() { Name = "Lenovo Ideapad" },
            new Laptop() { Name = "DELL XPS" },
            new Laptop() { Name = "HP Omen" }
        };
        NumberCounterTransient _numberCounterTransient;
        NumberCounterScoped _numberCounterScoped;
        NumberCounterSingleton _numberCounterSingleton;
        public HomeController(ILogger<HomeController> logger, 
                              NumberCounterTransient numberCounterTransient,
                              NumberCounterScoped numberCounterScoped,
                              NumberCounterSingleton numberCounterSingleton)
        {
            _numberCounterTransient = numberCounterTransient;
            _numberCounterScoped = numberCounterScoped;
            _numberCounterSingleton = numberCounterSingleton;
            _logger = logger;
        }

        public IActionResult Counter(bool doScoped)
        {
            if (true) new NumberCounterDependent()._numberCounterScoped.total += 1;

            CountersViewModel counterTotal = new() 
            {
                Transient = _numberCounterTransient.total += 1,
                Scoped = _numberCounterScoped.total += 1,
                Singleton = _numberCounterSingleton.total += 1
            };

            return View(counterTotal);
        }

        public IActionResult Index(string? searchString)
        {
            LaptopsViewModel result = new();
            result.LaptopList = _laptops;

            //TODO: search bind remains null in LaptopsViewModel.search
            if (searchString != null)
            {
                return View(result.GetByName(searchString));
            }

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
