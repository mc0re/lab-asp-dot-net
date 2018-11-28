using Globomantics.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Globomantics
{
    public class Startup
    {
        private IConfiguration mConfig;


        public Startup(IConfiguration config)
        {
            mConfig = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IConferenceService, ConferenceMemoryService>();
            services.AddSingleton<IProposalService, ProposalMemoryService>();
            services.Configure<GlobomanticsSettings>(mConfig.GetSection("Globomantics"));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();

            // Images and CSS
            app.UseStaticFiles();

            // app.UseMvcWithDefaultRoute() has default controller=Home, action=Index
            app.UseMvc(
                routes => routes.MapRoute("default", "{controller=Conference}/{action=Index}/{id?}")
                );

            //app.Use(async (context, next) =>
            //{
            //    logger.LogInformation("Before");
            //    await next();
            //    logger.LogInformation("After");
            //});

            //app.Run(async (context) =>
            //{
            //    logger.LogInformation("Second started");
            //    await context.Response.WriteAsync("Hello World!");
            //    logger.LogInformation("Second finished");
            //});
        }
    }
}
