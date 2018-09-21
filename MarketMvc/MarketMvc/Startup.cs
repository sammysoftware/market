using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc; //CacheProfile
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MarketMvc.Data;
using MarketMvc.Models;
using MarketMvc.Services;
using NorthwindEntitiesLib;

namespace MarketMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();//maybe can store the product list so it doesn't have to hit the database.

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AuthConnection")));

            // adds local Northwind DB
            services.AddDbContext<NorthwindDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("NorthwindConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services
                .AddMvc(options =>
                {
                    options.CacheProfiles.Add("Public5Minutes", new CacheProfile { Duration = 5 * 60, Location = ResponseCacheLocation.Any, VaryByHeader = "Accept-Language" });
                    options.CacheProfiles.Add("Public1Hour", new CacheProfile { Duration = 60 * 60, Location = ResponseCacheLocation.Any, VaryByHeader = "Accept-Language" });

                    //options.ValueProviderFactories.Add(new CookieValueProviderFactory());

                    //options.Filters.Add(new MiddlewareFilterAttribute(typeof(LocalizationPipeline)));
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
