using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Frutinet.Services.Identity.Users.Commands;
using RawRabbit.Extensions;
using RawRabbit.Configuration.Exchange;
using RawRabbit;

namespace Frutinet.Services.Identity.Controllers
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
            var commandId = Guid.NewGuid();
            await _busClient.PublishAsync(command, commandId, cfg => cfg
           .WithExchange(exchange => exchange.WithType(ExchangeType.Topic).WithName("Frutinet.Services.Identity"))
           .WithRoutingKey("user.create"));
            return await Task.FromResult(Accepted(command));
        }
    }
}