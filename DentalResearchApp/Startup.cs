using System;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DentalResearchApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; set; }
        public IHostingEnvironment Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddSessionStateTempDataProvider();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromHours(1); 
                    options.LoginPath = new PathString("/Login");
                    options.Cookie.HttpOnly = true;
                    options.Cookie.Name = "auth_cookie";
                });

            

            var connectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
            IContext context = new Context(connectionString);


            services.AddSingleton(context);

            //if (Environment.IsEnvironment("IntegrationTest"))
            //{
            //    IContext context = new TestContext();

            //    services.AddSingleton(context);
            //   // services.AddTransient<IContext, TestContext>();
            //}
            //else
            //{
            //    //services.AddTransient<IContext, ProdContext>();
            //}
            //services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsEnvironment("IntegrationTest"))
            {

            }
            else
            {
                app.UseHsts();
            }
            //app.UseSession();
            
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
