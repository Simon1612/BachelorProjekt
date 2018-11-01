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


            string linkDbName;
            string surveyDbName;
            string userDbName;
            string connectionString;

            if (Environment.IsEnvironment("IntegrationTest"))
            {
                linkDbName = Configuration.GetSection("LinkDbName").Value;
                surveyDbName = Configuration.GetSection("SurveyDbName").Value;
                userDbName = Configuration.GetSection("UserDbName").Value;
                connectionString = Configuration.GetSection("ConnectionString").Value;
            }
            else
            {
                linkDbName = Configuration.GetSection("MongoConnection:LinkDbName").Value;
                surveyDbName = Configuration.GetSection("MongoConnection:SurveyDbName").Value;
                userDbName = Configuration.GetSection("MongoConnection:UserDbName").Value;
                connectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
            }

            IContext context = new Context(connectionString, linkDbName, surveyDbName, userDbName);

            services.AddSingleton(context);
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
                app.UseHsts();
            }
            
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
