using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PlcApi.Entities;
using PlcApi.Middleware;
using PlcApi.Services;
using PlcApi.Services.EntityServices;
using PlcApi.Services.Interfaces;

namespace PlcApi
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
            services.AddDbContext<PlcDbContext>();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<PlcModelsSeeder>();
            services.AddScoped<ExceptionHandlingMiddleware>();
            services.AddSingleton<IPlcStorageService, PlcStorageService>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IPlcConnectionService, PlcConnectionService>();
            services.AddScoped<IPlcDataReadingService, PlcDataReadingService>();
            services.AddScoped<IInputOutputService, InputOutputService>();
            //services.AddScoped<IDiodeService, DiodeService>();
            services.AddScoped<IConveyorService, ConveyorService>();
            services.AddScoped<IPlcDataWritingService, PlcDataWritingService>();
            services.AddScoped<IPalletService, PalletService>();
            services.AddScoped<ISensorService, SensorService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlcApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, PlcModelsSeeder modelsSeeder)
        {
            modelsSeeder.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlcApi v1"));
            }
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
