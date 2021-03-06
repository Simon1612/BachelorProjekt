﻿using System;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Swashbuckle;
using Swashbuckle.AspNetCore.Swagger;

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
            string sessionDbName;
            string connectionString;

            if (Environment.IsEnvironment("IntegrationTest"))
            {
                linkDbName = Configuration.GetSection("LinkDbName").Value;
                surveyDbName = Configuration.GetSection("SurveyDbName").Value;
                userDbName = Configuration.GetSection("UserDbName").Value;
                sessionDbName = Configuration.GetSection("SessionDbName").Value;
                connectionString = Configuration.GetSection("ConnectionString").Value;
            }
            else
            {
                linkDbName = Configuration.GetSection("MongoConnection:LinkDbName").Value;
                surveyDbName = Configuration.GetSection("MongoConnection:SurveyDbName").Value;
                userDbName = Configuration.GetSection("MongoConnection:UserDbName").Value;
                sessionDbName = Configuration.GetSection("MongoConnection:SessionDbName").Value;
                connectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
            }

            var client = new MongoClient(connectionString);
            IContext context = new Context(client, linkDbName, surveyDbName, userDbName, sessionDbName);

            services.AddSingleton(context);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Dental Research App", Version = "v1" });
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
                app.UseHsts();
            }

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dental Research App v1");
            });

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
