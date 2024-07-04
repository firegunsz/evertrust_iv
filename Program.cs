using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Configuration;

namespace Relation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Nlog Start Exception");
                //throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var _env = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .Build().GetSection("Environment").Value;
            var builder = Host.CreateDefaultBuilder(args);

            builder.ConfigureWebHostDefaults(webBuilder =>
             {

                 webBuilder.UseContentRoot(System.IO.Directory.GetCurrentDirectory());
                 webBuilder.UseWebRoot("wwwroot");
                 webBuilder.UseStartup<Startup>().UseNLog();
                 webBuilder.ConfigureAppConfiguration((context, config) =>
                 {
                     switch (_env) 
                     {
                         case "PROD":
                             config.AddJsonFile("PROD.json");
                             break;
                         case "UAT":
                             config.AddJsonFile("UAT.json");
                             break;
                         case "LOCAL":
                             config.AddJsonFile("LOCAL.json");
                             break;
                     }
                  
                 });
             });

            builder.ConfigureLogging(logging =>
                 {
                     logging.ClearProviders();
                     logging.SetMinimumLevel(LogLevel.Trace);
                 }).UseNLog();

            return builder;
        }

    }
}
