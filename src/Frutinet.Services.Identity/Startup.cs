using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Frutinet.Common.Commands;
using Frutinet.Common.Mongo;
using Frutinet.Common.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Frutinet.Common.Extensions;
using RawRabbit.Configuration;
using Frutinet.Services.Identity.Configure;

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
            services.AddMongoDB(Configuration);

            services.AddMvc();

            var builder = new ContainerBuilder();

            builder.RegisterModule(new AutofacModule());

            builder.Populate(services);

            RabbitMqContainer.Register(builder, Configuration.GetSettings<RawRabbitConfiguration>());

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
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseMvc();
        }
    }
}