using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog;

using NSwag;
using NSwag.AspNetCore;

using MediatR;

using Newtonsoft.Json.Serialization;

using TournamentTracker.Api.ErrorLogger;
using TournamentTracker.Api.Filters;
using TournamentTracker.Api.Middleware;
using TournamentTracker.Common.Converters;
using TournamentTracker.Data.Contexts;
using TournamentTracker.Infrastructure.Helpers;

namespace TournamentTracker.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config")); //load nlog config
            Configuration = configuration;

            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = @$"C:\Users\airekeola\source\repos\TournamentTrackerLogs/${DateTime.Now.Date.ToShortDateString()}_logfile.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("TournamentTracker.Infrastructure");
            services.AddAutoMapper(assembly); //typeof(Startup)//AppDomain.CurrentDomain.GetAssemblies()
            services.AddSingleton<ILoggerManager, LoggerManager>();

            services.AddDbContext<TournamentTrackerWriteContext>();
            services.AddDbContext<TournamentTrackerReadContext>();
            //services.AddDbContext<TournamentTrackerWriteContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("TournamentTrackerWriteContext"), 
            //        b => b.MigrationsAssembly("TournamentTracker.Data")));

            //services.AddDbContext<TournamentTrackerReadContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("TournamentTrackerReadContext"), 
            //        b => b.MigrationsAssembly("TournamentTracker.Data")));


            // services.AddMediatR(Assembly.GetExecutingAssembly());
            //services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(ICustomerNameUpdateService).Assembly);

            services.AddScoped<IRandomizeHelper, RandomizeHelper>();
            services.AddMediatR(assembly);

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                options.SerializerSettings.Converters.Add(new TrimConverter());
            });
           // services.AddControllers().AddNewtonsoftJson();

            services.AddOpenApiDocument(config =>
            {
                config.PostProcess = doc =>
                {
                    doc.Info.Title = "Tournament Tracker API";
                    doc.Info.Version = "v1";
                    doc.Info.Description = "API Endpoint for the Tournament Tracker Solution";
                    doc.Info.Contact = new OpenApiContact
                    {
                        Name = "Afeez Irekeola",
                        Email = "",
                        Url = "http://github.com/hayfeez"
                    };

                    doc.Info.License = new OpenApiLicense
                    {
                        Name = "Free",
                        Url = ""
                    };

                    doc.Info.TermsOfService = "This endpoint is free to use";

                    //doc.Security = new List<OpenApiSecurityRequirement>
                    //{
                    //    new OpenApiSecurityRequirement
                    //    {

                    //    }
                    //};

                };

             //   config.OperationProcessors.Insert(0, new AddRequiredHeaderParameterOperationProcessor(Configuration));
                config.DocumentProcessors.Insert(0, new AddRequiredHeaderParameterDocumentProcessor(Configuration));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware>();
            //app.Map("/swagger", (appBuilder) =>
            //{
            //    appBuilder.UseMiddleware<SwaggerAuthMiddleware>(); //Add basic auth to swagger
            //});


            app.UseOpenApi();
           // app.UseSwaggerUi3();
            app.UseReDoc();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
