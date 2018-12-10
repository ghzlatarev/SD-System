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
using SD.Services.External;
using SD.Services.Data.Services.Contracts;
using SD.Services.Data.Services;
using Quartz.Spi;
using Quartz;
using Quartz.Impl;
using SD.Web.Utilities.Quartz;
using Newtonsoft.Json.Serialization;

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

            RegisterQuartzServices(services);

			services.AddSignalR();
            services.AddKendo();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IScheduler scheduler)
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

            scheduler.ScheduleJob(app.ApplicationServices.GetService<IJobDetail>(), app.ApplicationServices.GetService<ITrigger>());

            app.UseStaticFiles();

            app.UseAuthentication();

			app.UseSignalR(routes =>
			{
				routes.MapHub<NotificationHub>("/notificationHub");
			});

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
			services.AddTransient<INotificationService, NotificationService>();
			services.AddScoped<HttpClient>();
            services.AddScoped<IApiClient, ApiClient>();
        }

        private void RegisterServicesData(IServiceCollection services)
        {
            //Identity
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();

			//Custom
            services.AddScoped<ISensorService, SensorService>();
            services.AddScoped<ISensorDataService, SensorDataService>();
			services.AddScoped<IUserSensorService, UserSensorService>();
		}

		private void RegisterQuartzServices(IServiceCollection services)
        {
            //Quartz
            services.Add(new ServiceDescriptor(typeof(IJob), typeof(ScheduledJob), ServiceLifetime.Transient));
            services.AddSingleton<IJobFactory, ScheduledJobFactory>();
            services.AddSingleton<IJobDetail>(provider =>
            {
                return JobBuilder.Create<ScheduledJob>()
                  .WithIdentity("Sensors.job", "group1")
                  .Build();
            });

            services.AddSingleton<ITrigger>(provider =>
            {
                return TriggerBuilder.Create()
                .WithIdentity($"Sensors.trigger", "group1")
                .StartNow()
                .WithSimpleSchedule
                 (s =>
                    s.WithInterval(TimeSpan.FromSeconds(5))
                    .RepeatForever()
                 )
                 .Build();
            });

            services.AddSingleton<IScheduler>(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                scheduler.Start();
                return scheduler;
            });
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
            }).AddJsonOptions(o => o.SerializerSettings.ContractResolver = new DefaultContractResolver()); 
        }
    }
}
