using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Npgsql;

namespace LinkShorter
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
            services.AddControllers();

            var config = JObject.Parse(File.ReadAllText("config.json"));
            //overwrite values with env vars
            
            var databaseHost = Environment.GetEnvironmentVariable("database_host");
            if (databaseHost is not null)
            {
                config["database"]["host"] = databaseHost;
            }
            
            var databaseUsername = Environment.GetEnvironmentVariable("database_username");
            if (databaseUsername is not null)
            {
                config["database"]["username"] = databaseUsername;
            }
            
            var databasePassword = Environment.GetEnvironmentVariable("database_password");
            if (databasePassword is not null)
            {
                config["database"]["password"] = databasePassword;
            }
            
            var databaseName = Environment.GetEnvironmentVariable("database_name");
            if (databaseName is not null)
            {
                config["database"]["name"] = databaseName;
            }
            
            var urlBase = Environment.GetEnvironmentVariable("urlbase");
            if (urlBase is not null)
            {
                config["urlbase"] = urlBase;
            }
            
            var passwordPepper = Environment.GetEnvironmentVariable("password_pepper");
            if (passwordPepper is not null)
            {
                config["password_pepper"]= passwordPepper;
            }
            
            
            var configWrapper = new ConfigWrapper(config);
            services.AddSingleton(configWrapper);
            services.AddSingleton(new DatabaseWrapper(config));
            services.AddSwaggerGen();
            var stringGenerator = new StringGenerator();

            services.AddSingleton(stringGenerator);
            services.AddSingleton(pwd => new PasswordManager(stringGenerator, configWrapper));
            services.AddSingleton(SessionManager => new SessionManager(stringGenerator));
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stelz"); });
            }
            else
            {
                app.UseDefaultFiles();
                /*app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(env.ContentRootPath, "frontend-serve")),
                    RequestPath = ""
                });*/
            }
            
            

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors(
                options => options
                    .WithOrigins("http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            );
            app.UseRouting();


            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}