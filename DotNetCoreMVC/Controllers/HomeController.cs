using DotNetCoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using DataAccess;
using System.Linq;
using Hangfire;
using System;

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

        private readonly NumberCounterTransient _numberCounterTransient;
        private readonly NumberCounterScoped _numberCounterScoped;
        private readonly NumberCounterSingleton _numberCounterSingleton;

        private readonly NumberCounterDependent _numberCounterDependent;
        private readonly NumberCounterConfig _numberCounterConfig;

        private readonly DatabaseContext _context;

        private readonly IBackgroundJobClient _backgroundJobClient;

        public HomeController(NumberCounterTransient numberCounterTransient,
                              NumberCounterScoped numberCounterScoped,
                              NumberCounterSingleton numberCounterSingleton,
                              NumberCounterDependent numberCounterDependent,
                              IOptionsSnapshot<NumberCounterConfig> numberCounterConfig,
                              DatabaseContext databaseContext,
                              IBackgroundJobClient backgroundJobClient)
        {
            _numberCounterTransient = numberCounterTransient;
            _numberCounterScoped = numberCounterScoped;
            _numberCounterSingleton = numberCounterSingleton;

            _numberCounterDependent = numberCounterDependent;
            _numberCounterConfig = numberCounterConfig.Value;

            _context = databaseContext;

            _backgroundJobClient = backgroundJobClient;
        }
        public void GoHangfire(string? message)
        {
            message = message ?? "Hello from hangfire!";
            _backgroundJobClient.Enqueue(() => Console.WriteLine(message));
        }

        public IActionResult Counter(bool useTwoDependencies = false)
        {
            int IncrementAmount = _numberCounterConfig.IncrementAmount;

            if (useTwoDependencies)
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

        [Authorize]
        public async Task<IActionResult> Index(string? searchString)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            TestViewModel viewmodel = new();
            viewmodel.ProductTotal = _context.Stats.Select(x => x.NumberOfProducts).FirstOrDefault();
            viewmodel.LaptopList = _laptops;

            using (var client = new HttpClient())
            {
                client.SetBearerToken(token);
                var call_result = await client.GetAsync("https://localhost:5002/api/WeatherForecast");

                if (call_result.IsSuccessStatusCode)
                {
                    var message = await call_result.Content.ReadAsStringAsync();
                    viewmodel.AuthorizedMessageFromApi = message;
                }
                else 
                {
                    viewmodel.AuthorizedMessageFromApi = "mag niet";
                }
            }

            //TODO: search bind remains null in TestViewModel.search with [BindProperty] and asp-for
            if (searchString != null)
            {
                return View(viewmodel.GetByName(searchString));
            }

            return View(viewmodel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
