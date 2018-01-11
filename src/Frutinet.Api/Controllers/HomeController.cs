using Frutinet.Services.Identity.Users.Commands;
using Microsoft.AspNetCore.Mvc;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Api.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly IBusClient _busClient;

        public HomeController(IBusClient busClient)
        {
            _busClient = busClient;
        }

        [HttpGet]
        public IActionResult Get() => Content("Hello world");

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUser command)
        {
            await _busClient.PublishAsync(command);
            return Ok();
        }
    }
}