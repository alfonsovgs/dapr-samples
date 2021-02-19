using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using sample.microservice.reservation.Model;

namespace sample.microservice.reservation
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

            services.AddDaprClient();

            services.AddSingleton(new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, JsonSerializerOptions serializerOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapPost("reserve", Reserve);
            });

            async Task Reserve(HttpContext context)
            {
                Console.WriteLine("Enter Reservation");

                var client = context.RequestServices.GetRequiredService<DaprClient>();
                var item = await JsonSerializer.DeserializeAsync<Item>(context.Request.Body, serializerOptions);

                var sortedItem = new Item
                {
                    SKU = item.SKU,
                };

                sortedItem.Quantity -= item.Quantity;

                Console.WriteLine($"Reservation of {sortedItem.SKU} is now {sortedItem.Quantity}");

                context.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(context.Response.Body, sortedItem, serializerOptions);
            }
        }
    }
}
