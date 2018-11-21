using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;


namespace CityInfo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
            //    .AddJsonOptions(opt =>
            //{
            //    var reslv = opt.SerializerSettings.ContractResolver as DefaultContractResolver;
            //    if (reslv == null) return;

            //    reslv.NamingStrategy = null;
            //})
            ;
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
