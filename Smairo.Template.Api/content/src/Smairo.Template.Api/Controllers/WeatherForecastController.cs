using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Smairo.Template.Model.Repositories;

namespace Smairo.Template.Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDapperRepository _dapperRepository;
        private readonly IApiRepository _apiRepository;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDapperRepository dapperRepository, IApiRepository apiRepository)
        {
            _logger = logger;
            _dapperRepository = dapperRepository;
            _apiRepository = apiRepository;
        }

        /// <summary>
        /// Get weather
        /// </summary>
        /// <response code="200">Returns IEnumerable of <see cref="WeatherForecast"/></response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), (int) HttpStatusCode.OK)]
        public IActionResult Get()
        {
            var rng = new Random();
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Search weather
        /// </summary>
        /// <response code="200">Returns IEnumerable of <see cref="WeatherForecast"/></response>
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), (int) HttpStatusCode.OK)]
        public IActionResult Search([FromRoute] WeatherType type)
        {
            var rng = new Random();
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();

            throw new InvalidOperationException("Threw exception!");
        }
    }
}
