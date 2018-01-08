using Frutinet.Common.Commands;
using Frutinet.Common.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RawRabbit.Instantiation;
using Frutinet.Common.DependecyResolver;

namespace Frutinet.Common.RabbitMq
{
    public static class Extensions
    {
        public static Task WithCommandHandlerAsync<TCommand>(this IBusClient bus,
             IResolver resolver, string name = null) where TCommand : ICommand
        {
            return bus.SubscribeAsync<TCommand>(msg => resolver.Resolve<ICommandHandlerAsync<TCommand>>().HandleAsync(msg),
                ctx => ctx.UseConsumerConfiguration(
                    cfg => cfg.FromDeclaredQueue(q => q.WithName(GetExchangeName<TCommand>(name)))));
        }

        public static Task WithEventHandlerAsync<TEvent>(
            this IBusClient bus,
            IResolver resolver, string name = null) where TEvent : IEvent
        {
            return bus.SubscribeAsync<TEvent>(msg => resolver.Resolve<IEventHandlerAsync<IEvent>>().HandleAsync(msg),
                ctx => ctx.UseConsumerConfiguration(
                    cfg => cfg.FromDeclaredQueue(q => q.WithName(GetExchangeName<TEvent>(name)))));
        }

        private static string GetExchangeName<T>(string name = null)
            => string.IsNullOrWhiteSpace(name)
                ? $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}"
                : $"{name}/{typeof(T).Name}";

        public static void AddRabbitMq(this IServiceCollection service, IConfiguration configuration)
        {
            var options = new RabbitMqOptions();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(options);
            var client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions
            {
                ClientConfiguration = options
            });
            service.AddSingleton<IBusClient>(_ => client);
        }

        private static string GetQueueName<T>()
        {
            var a = Assembly.GetEntryAssembly().GetName();
            var b = typeof(T).Name;
            return $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}";
        }
    }
}