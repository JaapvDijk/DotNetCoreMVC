using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoAuthController : ControllerBase
    {
        /// <summary>
        /// Controller summary
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})s
            //.ToArray();
            return "Everyone can read this";
        }
    }
}
