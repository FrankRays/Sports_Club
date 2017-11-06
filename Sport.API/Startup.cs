using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sport.API.Entities;
using Sport.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Sport.API.Services;

namespace Sport.API
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=SportDB;Trusted_Connection=True";
            services.AddDbContext<SportContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<ISportRepository, SportRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            SportContext sportContext)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            sportContext.EnsureSeedDataForContext();

            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<Entities.Activity, Model.Activity>();
                cfg.CreateMap<Model.ActivityForCreationAndUpdate, Entities.Activity>();
            });

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
