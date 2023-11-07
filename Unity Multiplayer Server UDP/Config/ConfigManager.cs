using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity_Multiplayer_Server_UDP.Config
{
    public class ConfigManager
    {
        public bool SetupComplete { get; private set; } = false;
        public string? IpAddress { get; private set; }
        public int Port { get; private set; } = 11000;

        private static ConfigManager? instance;
        public static ConfigManager getInstance()
        {
            if (instance == null)
                instance = new ConfigManager();

            return instance;
        }

        private ConfigManager()
        {
            // Costruisce il sistema di configurazione
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            try
            {
                var appSettings = configuration.GetSection("AppSettings");
                if(appSettings != null)
                {
                    IpAddress = appSettings["IPAddress"];
                    string? portToOpen = appSettings["Port"];
                    if(portToOpen != null)
                        Port = int.Parse(portToOpen);
                }

                SetupComplete = true;
            } catch {
                SetupComplete = false;
            }
        }


    }
}
