﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API
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
           services.AddDbContext<DataContext>(x => 
           x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<DataContext>(x => x.UseSqlite("Connection String")); //Uncomment this if you are using sqlite and configure connection string in the appsettings 
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    // Ignores self referencing looping sincne we getting users-photos and users again.
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            services.AddCors();
            services.AddAutoMapper();

            // Register configurations for cloudinary and read settings from appSettings
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));

            /// AddSingleton Service is  Create a single instance of our repository for the first time, and reuse the same object in all course.
            //services.AddSingleton<IAuthRepository,AuthRepository>(); 

            /// AddTransient Service is  Created each time a request comes, a new instance is created. (for lightweight stateless services) 
            //services.AddTransient<IAuthRepository,AuthRepository>(); 

            /// AddScoped Service is created once per request within the scope for each HTTP request.
            services.AddScoped<IAuthRepository,AuthRepository>();
            services.AddScoped<IDatingRepository, DatingRepository>();
            services.AddTransient<Seed>();

            // configure application authentication scheme
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false

                });

            // register the LogUserActivity
            services.AddScoped<LogUserActivity>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            
            {
                //app.UseHsts();

                /* UseExceptionHandler Adds middleware to capture exceptions globally
                 * 
                 */
                app.UseExceptionHandler(builder => builder.Run(async context => {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message);
                    }
                }));
            }

            //seeder.SeedUsers();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
