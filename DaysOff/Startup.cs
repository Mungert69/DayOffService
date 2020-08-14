using ASPNETCoreScheduler.Scheduler;
using DayOff.Data;
using DaysOff.Models.LeelaBack;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;

namespace DayOff
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
            services.AddSingleton<IHostedService, ScheduleTask>();
            services.AddDbContext<DayOffContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<LeelaBackContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("LeelaDBConnection")));


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder.AllowAnyOrigin());
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {

            var cultureInfo = new CultureInfo("en-US");
           
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAnyOrigin");
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}/{id2?}/{id3?}/{id4?}/{id5?}/{id6?}/{id7?}/{id8?}");
            });
        }
    }
}
