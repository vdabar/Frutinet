using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore;
using RawRabbit;
using Frutinet.Common.Commands;
using Frutinet.Common.Events;
using Frutinet.Common.RabbitMq;
using Frutinet.Common.DependecyResolver;
using Autofac;

namespace Frutinet.Common.Services
{
    public class ServiceHost : IServiceHost
    {
        private readonly IWebHost _webHost;

        public ServiceHost(IWebHost webHost)
        {
            _webHost = webHost;
        }

        public void Run()
        {
            _webHost.Run();
        }

        public static HostBuilder Create<TStartup>(string[] args) where TStartup : class
        {
            Console.Title = typeof(TStartup).Namespace;
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseStartup<TStartup>();
            return new HostBuilder(webHostBuilder.Build());
        }

        public abstract class BuilderBase
        {
            public abstract ServiceHost Build();
        }

        public class HostBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;
            private IBusClient _busClient;
            private IResolver _resolver;

            public HostBuilder(IWebHost webHost)
            {
                _webHost = webHost;
                _resolver = new DefaultResolver(webHost);
            }

            public HostBuilder UseAutofac(ILifetimeScope scope)
            {
                _resolver = new AutofacResolver(scope);

                return this;
            }

            public BusBuilder UseRabbitMq(string queueName = null)
            {
                _busClient = _resolver.Resolve<IBusClient>();

                return new BusBuilder(_webHost, _busClient, _resolver, queueName);
            }

            public override ServiceHost Build()
            {
                return new ServiceHost(_webHost);
            }
        }

        public class BusBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;
            private IBusClient _bus;
            private readonly IResolver _resolver;
            private readonly string _queueName;

            public BusBuilder(IWebHost webHost, IBusClient bus, IResolver resolver, string queueName = null)
            {
                _webHost = webHost;
                _bus = bus;
                _resolver = resolver;
                _queueName = queueName;
            }

            public BusBuilder SubscribeToCommand<TCommand>(string exchangeName = null, string routingKey = null) where TCommand : ICommand
            {
                var commandHandler = _resolver.Resolve<ICommandHandlerAsync<TCommand>>();
                _bus.WithCommandHandlerAsync(commandHandler, exchangeName, routingKey);

                return this;
            }

            public BusBuilder SubscribeToEvent<TEvent>(string exchangeName = null, string routingKey = null) where TEvent : IEvent
            {
                var eventHandler = _resolver.Resolve<IEventHandlerAsync<TEvent>>();
                _bus.WithEventHandlerAsync(eventHandler, exchangeName, routingKey);

                return this;
            }

            public override ServiceHost Build()
            {
                return new ServiceHost(_webHost);
            }
        }
    }
}