using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD.Data.Models.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SD.Services.Data.Services.Identity.Contracts;
using SD.Services.Data.Services.Identity;
using System.Net.Http;
using SD.Data.Context;
using SD.Web.Utilities.Extensions;

namespace SD.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            HostingEnvironment = environment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            RegisterData(services);

            RegisterAuthentication(services);
            
            RegisterServicesExternal(services);
            RegisterServicesData(services);

            RegisterInfrastructure(services);
        }

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
                app.UseNotFoundExceptionHandler();
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "internalservererror",
                    template: "500",
                    defaults: new { controller = "Error", action = "InternalServerError" });

                routes.MapRoute(
                    name: "notfound",
                    template: "404",
                    defaults: new { controller = "Error", action = "PageNotFound" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });
        }

        private void RegisterData(IServiceCollection services)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                services.AddDbContext<DataContext>(options =>
                     options.UseSqlServer(Configuration.GetConnectionString("DevelopmentConnection")));
            }
            else
            {
                services.AddDbContext<DataContext>(options =>
                     options.UseSqlServer(Environment.GetEnvironmentVariable("AZURE_DS_DB_Connection")));
            }
        }

        private void RegisterAuthentication(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            if (this.HostingEnvironment.IsDevelopment())
            {
                services.Configure<IdentityOptions>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 0;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
                    options.Lockout.MaxFailedAccessAttempts = 999;
                });
            }

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Administrator");
                });

                options.AddPolicy("Default", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("User", "Administrator");
                });
            });
        }

        private void RegisterServicesExternal(IServiceCollection services)
        {
            services.AddScoped<HttpClient>();
        }

        private void RegisterServicesData(IServiceCollection services)
        {
            // Identity
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
        }

        private void RegisterInfrastructure(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddResponseCaching();
            services.AddMemoryCache();

            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        Duration = 60
                    });

                options.CacheProfiles.Add("Short",
                    new CacheProfile()
                    {
                        Duration = 30
                    });
            });
        }
    }
}
