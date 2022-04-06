using Currencies.Api.Middleware;
using Currencies.Data.Context;
using Currencies.Models;
using Currencies.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestSharp;
using System;

namespace Currencies.Api
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
            services.AddSwaggerGen();
            services.AddDbContext<DataContext>(x => x.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    serverDbContextOptionsBuilder =>
                    {
                        var minutes = (int)TimeSpan.FromMinutes(60).TotalSeconds;
                        serverDbContextOptionsBuilder.CommandTimeout(minutes);
                        serverDbContextOptionsBuilder.EnableRetryOnFailure();
                    })
            );

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var exchangeRatesApiSection = Configuration.GetSection("ExchangeSettings");
            services.Configure<ExchangeSettings>(exchangeRatesApiSection);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddScoped<ExchangeService, ExchangeService>();
            services.AddScoped<IRestClient, RestClient>(); //make signleton
            services.AddScoped<IRestRequest, RestRequest>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
