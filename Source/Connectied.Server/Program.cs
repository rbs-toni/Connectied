using System;
using System.Threading.Tasks;
using Connectied.Application;
using Connectied.Application.Persistence;
using Connectied.Infrastructure;
using Connectied.Server.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Connectied.Server;
public static class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich
            .FromLogContext()
            .WriteTo
            .Console()
            .CreateLogger();

        Log.Information("Starting Connectied API server");

        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddHttpClient<IGroupListsClient,GroupListsClient>(client => client.BaseAddress = new Uri("https://dummyjson.com/c/"));

            builder.Services.AddHostedService<GuestListsHostedService>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Connectied API", Description = "Connectied API" });
            });

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
            await dbInitializer.MigrateAsync();
            await dbInitializer.SeedAsync();

            app.UseExceptionHandler(opts => { });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapEndpoints();
            app.MapFallbackToFile("/index.html");
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
