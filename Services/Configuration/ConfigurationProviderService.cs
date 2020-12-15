using System;
using System.IO;
using Newtonsoft.Json;
using SpotPG.Services.Abstractions;
using SpotPG.Services.Configuration.Models;

namespace SpotPG.Services.Configuration
{
    public class ConfigurationProviderService : IConfigurationProviderService
    {
        public ServiceConfiguration Config { get; }

        private static readonly Lazy<ConfigurationProviderService> lazy = new(() => new ConfigurationProviderService());

        private readonly string configFilePath;

        public static ConfigurationProviderService Instance => lazy.Value;

        private ConfigurationProviderService()
        {
            this.configFilePath = Environment.GetEnvironmentVariable("SPOTPG_CONFIG_PATH") ?? "config.json";

            if (!File.Exists(this.configFilePath))
            {
                string initialConfig = JsonConvert.SerializeObject(new ServiceConfiguration(), Formatting.Indented);
                File.WriteAllText(this.configFilePath, initialConfig);
            }

            string content = File.ReadAllText(this.configFilePath);
            this.Config = JsonConvert.DeserializeObject<ServiceConfiguration>(content);
        }

        public void Save()
        {
            string config = JsonConvert.SerializeObject(this.Config, Formatting.Indented);
            File.WriteAllText(this.configFilePath, config);
        }
    }
}