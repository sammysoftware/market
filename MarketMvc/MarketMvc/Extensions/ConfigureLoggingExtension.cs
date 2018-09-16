using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MarketLogging;

namespace MarketMvc.Extensions
{
    public static class ConfigureLoggingExtension
    {
        public static ILoggingBuilder AddLoggingConfiguration(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
        {
            var loggingOptions = new Options.LoggingOptions();
            configuration.GetSection("Logging").Bind(loggingOptions);

            foreach (var provider in loggingOptions.Providers)
            {
                switch (provider.Name.ToLower())
                {
                    case "console":
                        {
                            loggingBuilder.AddConsole();
                            break;
                        }
                    case "file":
                        {
                            string filePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Logs", $"MarketMvc_{System.DateTime.Now.ToString("ddMMyyHHmm")}.log");
                            loggingBuilder.AddFile(filePath, (LogLevel)provider.LogLevel);
                            break;
                        }
//                    case "azureappservices":
//                        {
//                            AzureAppServiceDiagnosticExtension.AddAzureWebAppDiagnostics(configuration, loggingBuilder);
//                            break;
//                        }
                    default:
                        {
                            break;
                        }
                }
            }

            return loggingBuilder;
        }
    }
}
