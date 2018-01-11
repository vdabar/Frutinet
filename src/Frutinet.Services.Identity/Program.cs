using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Frutinet.Common.Services;
using Frutinet.Services.Identity.Users.Commands;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Frutinet.Services.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceHost.Create<Startup>(args)
                .UseAutofac(Startup.LifetimeScope)
                .UseRabbitMq(queueName: typeof(Program).Namespace)
                .SubscribeToCommand<CreateUser>(exchangeName: "Frutinet.Services.Users", routingKey: "user.create")
                .Build()
                .Run();
        }
    }
}