using System;
using System.Reflection;
using System.Threading.Tasks;
using Connectied.Application;
using Connectied.Application.Hubs;
using Connectied.Application.Persistence;
using Connectied.Infrastructure;
using Connectied.Server.Hubs;
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

            builder.Services
                .AddHttpClient<IGuestHttpClient, GuestHttpClient>(
                    client => client.BaseAddress = new Uri("https://dummyjson.com/c/"));

            builder.Services.AddSignalR();
            builder.Services.AddScoped<IGuestListNotifier, SignalRGuestListNotifier>();
            builder.Services.AddScoped<IGuestNotifier, SignalRGuestNotifier>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services
                .AddSwaggerGen(
                    options =>
                    {
                        options.SwaggerDoc(
                            "v1",
                            new OpenApiInfo
                            {
                                Version = "v1",
                                Title = "Connectied API",
                                Description = "Connectied API"
                            });
                        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                    });

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
            await dbInitializer.MigrateAsync();
            await dbInitializer.SeedAsync();

            app.UseExceptionHandler(
                opts =>
                {
                });

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
            app.MapHub<GuestListHub>("/hubs/guest-lists");
            app.MapHub<GuestHub>("/hubs/guests");
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
