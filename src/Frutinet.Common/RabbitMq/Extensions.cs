using Frutinet.Common.Commands;
using Frutinet.Common.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Frutinet.Common.DependecyResolver;

using RawRabbit;

using RawRabbit.Common;

namespace Frutinet.Common.RabbitMq
{
    public static class Extensions
    {
        public static ISubscription WithCommandHandlerAsync<TCommand>(this IBusClient bus,
           ICommandHandlerAsync<TCommand> handler, string exchangeName = null, string routingKey = null) where TCommand : ICommand
           => bus.SubscribeAsync<TCommand>(async (msg, context) => await handler.HandleAsync(msg),
           cfg => cfg.WithExchange(e => e.WithName(exchangeName)).WithRoutingKey(routingKey));

        public static ISubscription WithEventHandlerAsync<TEvent>(this IBusClient bus,
            IEventHandlerAsync<TEvent> handler, string exchangeName = null, string routingKey = null) where TEvent : IEvent
            => bus.SubscribeAsync<TEvent>(async (msg, context) => await handler.HandleAsync(msg),
            cfg => cfg.WithExchange(e => e.WithName(exchangeName)).WithRoutingKey(routingKey));
    }
}