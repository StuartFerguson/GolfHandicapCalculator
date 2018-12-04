using System;
using Microsoft.Extensions.Configuration;

namespace ManagementAPI.Service.Shared
{
    public static class ConfigurationReader
    {
        private const String AppSettings = "AppSettings";
        private static IConfigurationRoot ConfigurationRoot;
        
        public static Boolean IsInitialised { get; private set; }

        public static Uri GetBaseServerUri(String serviceName)
        {
            var uriString = ConfigurationReader.ConfigurationRoot.GetSection("AppSettings")[serviceName];

            return new Uri(uriString);
        }

        public static String GetConnectionString(String keyName)
        {
            return ConfigurationReader.GetValueFromSection("ConnectionStrings", keyName);
        }

        private static String GetValueFromSection(String sectionName, String keyName)
        {
            if (!ConfigurationReader.IsInitialised)
            {
                throw new InvalidOperationException("Configuration Reader has not been initialised");
            }
            IConfigurationSection section = ConfigurationReader.ConfigurationRoot.GetSection(sectionName);
            if (section == null)
            {
                throw new Exception($"Section [{sectionName}] not found.");
            }

            if (section[keyName] == null)
            {
                throw new Exception($"No configuration value was found for key [{sectionName}:{keyName}]");
            }

            return section[keyName];
        }

        public static String GetValue(String keyName)
        {
            return ConfigurationReader.GetValueFromSection("AppSettings", keyName);
        }

        public static void Initialise(IConfigurationRoot configurationRoot)
        {
            ConfigurationRoot = configurationRoot ?? throw new ArgumentNullException(nameof(configurationRoot));
            ConfigurationReader.IsInitialised = true;
        }
    }
}