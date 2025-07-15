using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoAuction.Application;
using AutoAuction.Domain.Repositories;
using AutoAuction.Infrastructure.Repositories;
using AutoAuction.CrossCutting.Logging;

namespace AutoAuction.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register application services
            builder.Services.AddSingleton<IAuctionRepository, AuctionRepository>();
            builder.Services.AddSingleton<AuctionService>();
            builder.Services.AddSingleton<ILogger, Logger>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
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
