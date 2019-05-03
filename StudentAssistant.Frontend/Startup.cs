using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace StudentAssistant.Frontend
{
    public class Startup
    {
        public const string url = "http://localhost:18936"; // @"http://192.168.1.224:7500";

        public void ConfigureServices(IServiceCollection services)
        {

        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
