using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using CoreMVCWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreMVCWebAPI
{
    public class Startup
    {
        //Add a Startup() constructor method and link it to appsettings.js
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsetings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public IConfigurationRoot Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //inject repository into DI container, use database in memory
            //services.AddDbContext<TodoContext>(options => options.UseInMemoryDatabase().UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            //inject repository into DI container, and use sql databse
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));


            //The first generic type represents the type (typically an interface) that will be requested from the container. 
            //The second generic type represents the concrete type that will be instantiated by the container and used to fulfill such requests.
            services.AddScoped<ITodoRepository, TodoRepository>();

            //add mvc service to container, this is conventional routing
            //This also applys to web api which is Attribute Routing
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Use MVC route in HTTP request pipeline
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                    );
            });

        }
    }
}
