using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;


namespace VersionedApiLab
{
    public class Startup
    {
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // This setting or CompatibilityVersion.Version_2_1 are needed
            // to get versioning to work at the moment
            // https://github.com/Microsoft/aspnet-api-versioning/issues/363
            services.AddMvc(opt => opt.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddApiVersioning(cfg =>
            {
                cfg.DefaultApiVersion = new ApiVersion(1, 1);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = true;

                // By default it's a query parameter "api-version".
                cfg.ApiVersionReader = new QueryStringApiVersionReader("api-ver", "api-version");
                // This one changes to control to one of the specified headers.
                cfg.ApiVersionReader = new HeaderApiVersionReader("x-ver", "x-version");

                //Another way is conventions defined here instead of on the controllers.
                cfg.Conventions.Controller<RandomController>()
                    .HasApiVersion(new ApiVersion(1, 0))
                    .HasApiVersion(new ApiVersion(1, 1))
                    .HasApiVersion(new ApiVersion(1, 2))
                    .Action(m => m.GetNumberAsObject()).MapToApiVersion(new ApiVersion(1, 2));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Versioned API",
                    Version = "v1",
                    Description = "Versioned API.",
                    Contact = new OpenApiContact { Name = "MIN" },
                    License = new OpenApiLicense { Name = "Not copyrighted" }
                });

                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                //var commentPath = Path.Combine(basePath, "VersionedApiLab.xml");
                //c.IncludeXmlComments(commentPath);
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Versioned API");
            });
            app.UseApiVersioning();
            app.UseMvc();
        }
    }
}
