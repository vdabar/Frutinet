﻿using System;
using System.IO;
using Autofac;
using Serilog;
using Polly;
using RabbitMQ.Client.Exceptions;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.Instantiation;

namespace Frutinet.Common.RabbitMq
{
    public static class RabbitMqContainer
    {
        private static readonly ILogger Logger = Log.Logger;

        public static void Register(ContainerBuilder builder, RawRabbitConfiguration configuration, int retryAttempts = 5)
        {
            var policy = Policy
                .Handle<ConnectFailureException>()
                .Or<BrokerUnreachableException>()
                .Or<IOException>()
                .WaitAndRetry(retryAttempts, retryAttempt =>
                            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Logger.Error(exception, "Can not connect to RabbitMQ. " +
                                                $"Retries: {retryCount}, duration: {timeSpan}");
                    }
                );
            builder.RegisterInstance(configuration).SingleInstance();
            policy.Execute(() =>
            {
                builder.Register(context => RawRabbitFactory.CreateInstanceFactory(new RawRabbitOptions
                {
                    DependencyInjection = ioc => ioc.AddSingleton(configuration)
                }))
                .As<IInstanceFactory>()
                .SingleInstance();
                builder.Register(context => context.Resolve<IInstanceFactory>().Create());
            });
        }
    }
}