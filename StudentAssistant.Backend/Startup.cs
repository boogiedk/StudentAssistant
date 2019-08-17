﻿using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentAssistant.Backend.Models;
using StudentAssistant.Backend.Models.Email;
using StudentAssistant.Backend.Services;
using StudentAssistant.Backend.Services.Implementation;
using StudentAssistant.DbLayer.Services;
using StudentAssistant.DbLayer.Services.Implementation;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using StudentAssistant.Backend.Infrastructure;
using StudentAssistant.Backend.Models.ParityOfTheWeek;
using StudentAssistant.DbLayer.Models.CourseSchedule;
using Swashbuckle.AspNetCore.Swagger;

namespace StudentAssistant.Backend
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;

            var builder = new ConfigurationBuilder()
                            .SetBasePath(Path.Combine(env.ContentRootPath, "Infrastructure", "Configuration"))
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
            _configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>();
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<Context>()
                    .AddDefaultTokenProviders()
                    .AddDefaultTokenProviders();

            #region Scoped

            services.AddScoped<IParityOfTheWeekService, ParityOfTheWeekService>();
            services.AddScoped<IUserSupportService, UserSupportService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<ICourseScheduleService, CourseScheduleService>();
            services.AddScoped<ICourseScheduleFileService, CourseScheduleFileService>();
            services.AddScoped<IImportDataExcelService, ImportDataExcelService>();
            services.AddScoped<IImportDataJsonService, ImportDataJsonService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<ICourseScheduleDatabaseService, CourseScheduleDatabaseService>();
            services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();

            #endregion

            #region Culture

            var cultureInfo = new CultureInfo("ru-RU");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


            #endregion

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

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            //services.Configure<EmailServiceConfigurationModel>(options => Configuration.GetSection("EmailServiceConfigurationModel").Bind(options));
            //services.Configure<ParityOfTheWeekConfigurationModel>(options => Configuration.GetSection("ParityOfTheWeekConfigurationModel").Bind(options));
            //services.Configure<CourseScheduleDataServiceConfigurationModel>(Configuration.GetSection("ListCourseSchedule"));

            services.Configure<EmailServiceConfigurationModel>(options => _configuration.GetSection("EmailServiceConfigurationModel").Bind(options));
            services.Configure<ParityOfTheWeekConfigurationModel>(options => _configuration.GetSection("ParityOfTheWeekConfigurationModel").Bind(options));
            services.Configure<CourseScheduleDataServiceConfigurationModel>(_configuration.GetSection("ListCourseSchedule"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "StudentAssistant API",
                    Description = "A StudentAssistant ASP.NET Core Web API",
                    License = new License
                    {
                        Name = "MIT License",
                        Url = "https://github.com/boogiedk/StudentAssistant/blob/master/LICENSE"
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAutoMapper();
            services.AddMvc().AddJsonOptions(options =>
            {
                var camelResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ContractResolver = camelResolver;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentAssistant API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
