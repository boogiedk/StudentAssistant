using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.Models.ConfigurationModels;
using StudentAssistant.Backend.Models.Email;
using StudentAssistant.Backend.Services;
using StudentAssistant.Backend.Services.Implementation;
using StudentAssistant.DbLayer.Services;
using StudentAssistant.DbLayer.Services.Implementation;
using System.Collections.Generic;

namespace StudentAssistant.Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath + "\\Infrastructure\\Configuration")
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                            .AddJsonFile($"EmailServiceConfigurationModel.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"ParityOfTheWeekConfigurationModel.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"CourseScheduleDataServiceConfigurationModel.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddScoped<IParityOfTheWeekService, ParityOfTheWeekService>();
            services.AddScoped<IUserSupportService, UserSupportService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<ICourseScheduleService, CourseScheduleService>();
            services.AddScoped<ICourseScheduleDataService, CourseScheduleDataService>();

            services.Configure<EmailServiceConfigurationModel>(options => Configuration.GetSection("EmailServiceConfigurationModel").Bind(options));
            services.Configure<ParityOfTheWeekConfigurationModel>(options => Configuration.GetSection("ParityOfTheWeekConfigurationModel").Bind(options));

            services.Configure<CourseScheduleDataServiceConfigurationModel>(Configuration.GetSection("ListCourseSchedule"));

         //   services.Configure<CourseScheduleDataServiceConfigurationModel>(options =>
         //   options.ListCourseScheduleDatabaseModel = Configuration.GetSection("ListCourseScheduleDatabaseModel").Get<List<CourseScheduleDatabaseModel>>());

            services.AddAutoMapper();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
