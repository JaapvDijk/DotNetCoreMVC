using DotNetCoreMVC.Models;
using DotNetCoreMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;

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

        private readonly ITokenService _tokenService;

        public HomeController(NumberCounterTransient numberCounterTransient,
                              NumberCounterScoped numberCounterScoped,
                              NumberCounterSingleton numberCounterSingleton,
                              NumberCounterDependent numberCounterDependent,
                              IOptionsSnapshot<NumberCounterConfig> numberCounterConfig,
                              ITokenService tokenService)
        {
            _numberCounterTransient = numberCounterTransient;
            _numberCounterScoped = numberCounterScoped;
            _numberCounterSingleton = numberCounterSingleton;

            _numberCounterDependent = numberCounterDependent;
            _numberCounterConfig = numberCounterConfig.Value;

            _tokenService = tokenService;
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
            TestViewModel viewmodel = new();
            viewmodel.LaptopList = _laptops;

            using (var client = new HttpClient())
            {
                //client.RequestClientCredentialsTokenAsync()
                //{ }
                //TODO: IDisposable?
                var tokenResponse = await _tokenService.GetToken("weatherapi.read");
                viewmodel.MyToken = tokenResponse.AccessToken;
                client.SetBearerToken(tokenResponse.AccessToken);

                var call_result = await client.GetAsync("https://localhost:5002/WeatherForecast");
                var a = 1;
                if (call_result.IsSuccessStatusCode)
                {
                    var message = await call_result.Content.ReadAsStringAsync();
                    viewmodel.AuthorizedMessageFromApi = message;
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
