using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace StudentAssistant.Backend
{
    public class Program
    {
        public static void Main(string[] args) => BuildWebHost(args).Run();

        public static IWebHost BuildWebHost(string[] args)
        {
            var builtConfig = new ConfigurationBuilder()
                .AddJsonFile(@"C:\Users\ganz1\Desktop\VS Projects\StudentAssistant\StudentAssistant.Backend\Infrastructure\NLog\nlog.configappsettings.json")
                .Build();

            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                    logging.AddConfiguration(builtConfig);
                })
                .UseNLog()
                .Build();
        }
    }
}