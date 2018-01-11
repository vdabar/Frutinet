using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetSettings<T>(this IConfiguration configuration, string section = "") where T : new()
        {
            if (string.IsNullOrWhiteSpace(section))
            {
                section = typeof(T).Name.Replace("Settings", string.Empty).Replace("Configuration", string.Empty);
            }

            var configurationValue = new T();
            configuration.GetSection(section).Bind(configurationValue);

            return configurationValue;
        }
    }
}