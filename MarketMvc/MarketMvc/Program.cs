using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MarketMvc.Extensions;

namespace MarketMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging((hostingcontext, logging) =>
                {
                    logging.AddLoggingConfiguration(hostingcontext.Configuration);
                })
                //.UseIISIntegration() //only use this when hosted by Elastic Beanstalk
                .Build();
    }
}
