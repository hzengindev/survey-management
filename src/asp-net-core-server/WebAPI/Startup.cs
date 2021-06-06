using Business.Abstract.Image;
using Business.Abstract.Survey;
using Business.Concrete.Image;
using Business.Concrete.Survey;
using Core.Utilities.Caching;
using Core.Utilities.Caching.Microsoft;
using Core.Utilities.Configurations;
using Core.Utilities.Dynamics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
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

            AppSettings.Logging = Configuration.GetSection("Logging").Get<LoggingConfig>();
            AppSettings.Dynamics = Configuration.GetSection("Dynamics").Get<DynamicsConfig>();

            services.AddMemoryCache();

            services.AddSingleton(sp => new ServiceClientWrapper(AppSettings.Dynamics.URI, AppSettings.Dynamics.ClientId, AppSettings.Dynamics.Secret));
            services.AddTransient<IOrganizationService, ServiceClient>(sp => sp.GetService<ServiceClientWrapper>().ServiceClient.Clone());

            services.AddScoped<ISurveyService, SurveyManager>();
            services.AddScoped<IGetSurvey, GetSurveyHandler>();
            services.AddScoped<ICompleteSurvey, CompleteSurveyHandler>();
            services.AddScoped<IGetImage, GetImageHandler>();

            services.AddSingleton<ICacheService, MemoryCacheManager>();

            services.AddCors(option =>
            {
                option.AddPolicy("default", config =>
                {
                    config.AllowAnyOrigin();
                    config.AllowAnyHeader();
                    config.AllowAnyMethod();
                });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("default");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
