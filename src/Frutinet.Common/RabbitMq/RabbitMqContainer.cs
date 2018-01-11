using System;
using System.IO;
using Autofac;
using Serilog;
using Polly;
using Frutinet.Common.Extensions;
using RabbitMQ.Client.Exceptions;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.vNext;

namespace Frutinet.Common.RabbitMq
{
    public static class RabbitMqContainer
    {
        private static readonly ILogger Logger = Log.Logger;

        public static void Register(ContainerBuilder builder, RawRabbitConfiguration rawRabbitConfiguration, int retryAttempts = 5)
        {
            var rmqRetryPolicy = Policy
               .Handle<ConnectFailureException>()
               .Or<BrokerUnreachableException>()
               .Or<IOException>()
               .WaitAndRetry(5, retryAttempt =>
                   TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                   (exception, timeSpan, retryCount, context) =>
                   {
                       Logger.Error(exception, "Can not connect to RabbitMQ. " +
                                                $"Retries: {retryCount}, duration: {timeSpan}");
                   }
               );
            builder.RegisterInstance(rawRabbitConfiguration).SingleInstance();
            rmqRetryPolicy.Execute(() => builder
                    .RegisterInstance(BusClientFactory.CreateDefault(rawRabbitConfiguration))
                    .As<IBusClient>()
            );
        }
    }
}