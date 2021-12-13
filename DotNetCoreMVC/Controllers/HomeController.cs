using DotNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DotNetCoreMVC.Controllers
{
    public class HomeController : Controller
    {
        //In memory data
        private readonly List<Laptop> _laptops = new()
        {
            new Laptop() { Name = "Lenovo Ideapad" },
            new Laptop() { Name = "DELL XPS" },
            new Laptop() { Name = "HP Omen" }
        };

        private NumberCounterTransient _numberCounterTransient;
        private NumberCounterScoped _numberCounterScoped;
        private NumberCounterSingleton _numberCounterSingleton;

        private NumberCounterDependent _numberCounterDependent;
        private NumberCounterConfig _numberCounterConfig;
        public HomeController(NumberCounterTransient numberCounterTransient,
                              NumberCounterScoped numberCounterScoped,
                              NumberCounterSingleton numberCounterSingleton,
                              NumberCounterDependent numberCounterDependent,
                              IOptionsSnapshot<NumberCounterConfig> numberCounterConfig)
        {
            _numberCounterTransient = numberCounterTransient;
            _numberCounterScoped = numberCounterScoped;
            _numberCounterSingleton = numberCounterSingleton;

            _numberCounterDependent = numberCounterDependent;
            _numberCounterConfig = numberCounterConfig.Value;
        }

        public IActionResult Counter(bool UseTwoDependencies = false)
        {
            int IncrementAmount = _numberCounterConfig.IncrementAmount;

            if (UseTwoDependencies)
            {
                _numberCounterDependent.NumberCounterScoped.total += IncrementAmount;
                _numberCounterDependent.NumberCounterSingleton.total += IncrementAmount;
                _numberCounterDependent.NumberCounterTransient.total += IncrementAmount;
            }

            CountersViewModel counterTotal = new() 
            {
                Transient = _numberCounterTransient.total += IncrementAmount,
                Scoped = _numberCounterScoped.total += IncrementAmount,
                Singleton = _numberCounterSingleton.total += IncrementAmount
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
