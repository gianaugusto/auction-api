using AutoAuction.Application;
using AutoAuction.CrossCutting.Logging;
using AutoAuction.Domain.Repositories;
using AutoAuction.Infrastructure;
using AutoAuction.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoAuction.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AutoAuction API", Version = "v1" });

                // Define the BearerAuth scheme that's in use
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            // Register my DB
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open(); // keep open!

            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseSqlite(connection));

            // Register application services
            builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
            builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
            builder.Services.AddScoped<IAuctionService, AuctionService>();
            builder.Services.AddScoped<IInventoryService, InventoryService>();
            builder.Services.AddSingleton<ILogger, Logger>();
            builder.Services.AddScoped<DatabaseSeeder>();
            builder.Services.AddScoped<IntegrationTestSeeder>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                // Seed the database
                using (var scope = app.Services.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
                    await seeder.SeedAsync();
                }
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Use exception handling middleware
            app.UseMiddleware<ExceptionMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
