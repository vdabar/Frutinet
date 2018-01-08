using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using System.Reflection;
using Frutinet.Services.Identity.Users.Commands;
using Frutinet.Services.Identity.Users.Repositories;

namespace Frutinet.Services.Identity
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // var infrastructureAssembly = typeof(AggregateRoot).GetTypeInfo().Assembly;
            var domainAssembly = typeof(CreateUser).GetTypeInfo().Assembly;
            var dataAssembly = typeof(IUserRepository).GetTypeInfo().Assembly;
            //var reportingAssembly = typeof(GetMaze).GetTypeInfo().Assembly;
            //builder.RegisterAssemblyTypes(infrastructureAssembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(domainAssembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(dataAssembly).AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(reportingAssembly).AsImplementedInterfaces();
        }
    }
}