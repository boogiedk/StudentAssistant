using System;
using System.ComponentModel;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentAssistant.Backend.Models.Email;
using StudentAssistant.Backend.Services;
using StudentAssistant.Backend.Services.Implementation;
using StudentAssistant.DbLayer.Services;
using StudentAssistant.DbLayer.Services.Implementation;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using StudentAssistant.Backend.Infrastructure;
using StudentAssistant.Backend.Infrastructure.AutoMapper;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.ParityOfTheWeek;
using StudentAssistant.DbLayer;
using StudentAssistant.DbLayer.Interfaces;
using StudentAssistant.DbLayer.Models;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using Swashbuckle.AspNetCore.Swagger;


namespace StudentAssistant.Backend
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(env.ContentRootPath, "Infrastructure", "Configuration"))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("EmailServiceConfigurationModel.json", optional: true, reloadOnChange: true)
                .AddJsonFile("CourseScheduleDataServiceConfigurationModel.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            #region Logger
            
            NLogBuilder.ConfigureNLog(Path.Combine(env.ContentRootPath, "Infrastructure", "NLog", "nlog.config"));

            LogManager.Configuration.Variables["appdir"] =
                Path.Combine(env.ContentRootPath, "Storages", "Nlog",
                    " "); // add empty path for create dir linux/windows

            #endregion
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Authentication

            services.AddDbContext<ApplicationDbContext>();
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    const string key = "q7fs8DDw823hSyaNYCKsa02";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuer = false
                    };
                });

            #endregion

            #region Mapper

            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new AutoMapperConfiguration()); });

            IMapper mapper = mappingConfig.CreateMapper();

            #endregion

            #region Scoped

            services.AddScoped<IParityOfTheWeekService, ParityOfTheWeekService>();
            services.AddScoped<IUserSupportService, UserSupportService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<ICourseScheduleService, CourseScheduleService>();
            services.AddScoped<ICourseScheduleFileService, CourseScheduleFileService>();
            services.AddScoped<IImportDataExcelService, ImportDataExcelService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IControlWeekService, ControlWeekService>();
            services.AddScoped<IExamScheduleService, ExamScheduleService>();
            services.AddScoped<ICourseScheduleDatabaseService, CourseScheduleDatabaseService>();
            services.AddScoped<IControlWeekDatabaseService, ControlWeekDatabaseService>();
            services.AddScoped<IExamScheduleDatabaseService, ExamScheduleDatabaseService>();

            services.AddSingleton(mapper);

            #endregion

            #region Culture

            var cultureInfo = new CultureInfo("ru-RU");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            #endregion

            #region Cors
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                );
            });
            
            #endregion

            #region Configure

            services.Configure<EmailServiceConfigurationModel>(options =>
                _configuration.GetSection("EmailServiceConfigurationModel").Bind(options));
            services.Configure<ParityOfTheWeekConfigurationModel>(options =>
                _configuration.GetSection("ParityOfTheWeekConfigurationModel").Bind(options));
            services.Configure<CourseScheduleDataServiceConfigurationModel>(
                _configuration.GetSection("ListCourseSchedule"));

            #endregion

            #region Swagger
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "StudentAssistant API",
                    Description = "A StudentAssistant ASP.NET Core Web API",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            
            #endregion
            
            services.AddMvc().AddJsonOptions(option => { option.JsonSerializerOptions.MaxDepth = 256; });
            
          //  services.AddControllers();
        }
        
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env
        )
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentAssistant API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseCors("CorsPolicy");
            
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}