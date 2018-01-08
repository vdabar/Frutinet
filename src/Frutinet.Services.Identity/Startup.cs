using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Frutinet.Common.Commands;
using Frutinet.Common.Events;
using Frutinet.Common.Services;
using Frutinet.Services.Identity.Users.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client.Exceptions;
using RawRabbit.Configuration;

namespace Frutinet.Services.Identity
{
    public class Startup
    {
        public string EnvironmentName { get; set; }
        public IConfiguration Configuration { get; }
        public static ILifetimeScope LifetimeScope { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var rmqRetryPolicy = Policy
              .Handle<ConnectFailureException>()
              .Or<BrokerUnreachableException>()
              .Or<IOException>()
              .WaitAndRetry(5, retryAttempt =>
                  TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                  (exception, timeSpan, retryCount, context) =>
                  {
                      //logger.LogError(new EventId(10001, "RabbitMQ Connect Error"), exception, $"Cannot connect to RabbitMQ. retryCount:{retryCount}, duration:{timeSpan}");
                  }
              );

            var builder = new ContainerBuilder();

            builder.Populate(services);

            var assembly = typeof(Startup).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IEventHandlerAsync<>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandlerAsync<>));
            builder.RegisterType<UserRepository>().As<IUserRepository>();

            LifetimeScope = builder.Build().BeginLifetimeScope();

            return new AutofacServiceProvider(LifetimeScope);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}