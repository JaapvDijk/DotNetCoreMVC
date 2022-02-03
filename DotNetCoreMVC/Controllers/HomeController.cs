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
using Nest;

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

        private IElasticClient _elasticClient { get; }

        public HomeController(NumberCounterTransient numberCounterTransient,
                              NumberCounterScoped numberCounterScoped,
                              NumberCounterSingleton numberCounterSingleton,
                              NumberCounterDependent numberCounterDependent,
                              IOptionsSnapshot<NumberCounterConfig> numberCounterConfig,
                              DatabaseContext databaseContext,
                              IElasticClient elasticClient)
        {
            _numberCounterTransient = numberCounterTransient;
            _numberCounterScoped = numberCounterScoped;
            _numberCounterSingleton = numberCounterSingleton;

            _numberCounterDependent = numberCounterDependent;
            _numberCounterConfig = numberCounterConfig.Value;

            _context = databaseContext;
            _elasticClient = elasticClient;
        }

        [Authorize]
        public async Task<IActionResult> Authorized()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            using (var client = new HttpClient())
            {
                client.SetBearerToken(token);
                var call_result = await client.GetAsync("https://localhost:5002/api/WeatherForecast");

                if (call_result.IsSuccessStatusCode)
                {
                    var message = await call_result.Content.ReadAsStringAsync();
                    return View(message);
                }
            }

            return View("U authorized m8!");
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

        public async Task<IActionResult> Index(string? searchString)
        {
            TestViewModel viewmodel = new();
            viewmodel.ProductTotal = _context.Stats.Select(x => x.NumberOfProducts).FirstOrDefault();
            viewmodel.LaptopList = _laptops;

            //TODO: search bind remains null in TestViewModel.search with [BindProperty] and asp-for
            if (searchString != null)
            {
                return View(viewmodel.GetByName(searchString));
            }

            return View(viewmodel);
        }
        public async Task<IActionResult> Elastic(string id)
        {
            //var response = await _elasticClient.SearchAsync<User>(
            //    s => s.Query( q => q.Term(t => t.Name, id) ||
            //         q.Match(m => m.Field(
            //            f => f.Name).Query(id)))
            //    );

            //var response = _elasticClient.Search<User>(s => s
            //.Index("users")
            //.Query(q => q
            //    .Term(o => o.Name, "test")));

            var response = await _elasticClient.GetAsync<User>(new DocumentPath<User>(
                new Id(id)), x => x.Index("users"));

            return View(response.Source);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
