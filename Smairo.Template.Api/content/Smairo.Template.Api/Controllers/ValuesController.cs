using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smairo.Template.Api.Services;
namespace Smairo.Template.Api.Controllers
{
    [Route("api/v1/values")]
    public class ValuesController : Controller
    {
        private readonly IValuesService _valuesService;
        public ValuesController(IValuesService valuesService)
        {
            _valuesService = valuesService;
        }

        // GET api/values
        [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _valuesService.GetValuesAsync());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _valuesService.GetValueAsync(id));
        }

        // POST api/values
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]string value)
        {
            await _valuesService.PostValuesAsync(value);
            return Created("", value);
        }
    }
}