using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SecureApiLab
{
    public class Startup
    {
        private IConfiguration mConfiguration;


        public Startup(IConfiguration configuration)
        {
            mConfiguration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(opt => opt.Filters.Add(new RequireHttpsAttribute()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddIdentity<ApiUser, IdentityRole>();
            services.AddSingleton<IUserStore<ApiUser>, ApiUserStore<ApiUser>>();
            services.AddSingleton<IRoleStore<IdentityRole>, ApiRoleStore<IdentityRole>>();
            services.AddScoped<SeedAuth>();
            services.AddAuthentication(opt => opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Audience = mConfiguration["Tokens:Site"];
                    opt.ClaimsIssuer = mConfiguration["Tokens:Site"];
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireSignedTokens = true,
                        ValidateIssuer = true,
                        ValidIssuer = mConfiguration["Tokens:Site"],
                        ValidateAudience = true,
                        ValidAudience = mConfiguration["Tokens:Site"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mConfiguration["Tokens:Key"])),
                        ValidateLifetime = true
                    };
                    opt.SaveToken = true;
                });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetService<SeedAuth>().SeedAsync().Wait();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
