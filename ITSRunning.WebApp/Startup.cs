using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ITSRunning.WebApp.Data;
using ITSRunning.WebApp.Models;
using ITSRunning.WebApp.Services;
using Microsoft.Extensions.Hosting;
using ITSRunning.DataAccess.Runners;
using ITSRunning.DataAccess.Activities;
using ITSRunning.DataAccess.Telemetries;

namespace RunningApp
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
            var cs = Configuration.GetConnectionString("SqlServer");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(cs));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
            services.AddSingleton<IRunnerRepository>(new RunnerRepository(cs));
            services.AddSingleton<IActivityRepository>(new ActivityRepository(cs));
            services.AddSingleton<ITelemetryRepository>(new TelemetryRepository(cs));
            services.AddSingleton<IHostedService, TopicListenerService>();
            services.AddSingleton<ISignalRRegistry, SignalRRegistry>();
            services.AddSingleton<ITelemetrySender, TelemetrySender>();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
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

            app.UseSignalR(routes =>
            {
                routes.MapHub<TopicListenerHub>("/topiclistener");
            });
        }
    }
}
