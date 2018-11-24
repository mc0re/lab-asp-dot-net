using CityInfo.Entities;
using CityInfo.Models;
using CityInfo.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;

namespace CityInfo
{
    public class Startup
    {
        public static IConfiguration Configuration;


        public Startup(IConfiguration conf)
        {
            Configuration = conf;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(opt => opt.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()))
                //.AddJsonOptions(opt =>
                //{
                //    if (!(opt.SerializerSettings.ContractResolver is DefaultContractResolver reslv)) return;

                //    reslv.NamingStrategy = null;
                //})
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PoiValidator>())
            ;
            services.AddTransient<ISenderService, LocalMailService>();
            var connString = Configuration["DatabaseConnection:CityInfo"];
            services.AddDbContext<CityInfoContext>(opt => opt.UseSqlServer(connString));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {
            logger.AddConsole();
            logger.AddNLog();

            // Debug is registered by default
            //logger.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();
            app.UseMvc(
                //config =>
                //{
                //    config.MapRoute(
                //        name: "Default",
                //        template: "{controller}/{action}/{id?}",
                //        defaults: new { controller = "Home", action = "Index" });
                //}
                );

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
