using Connectied.Application.Persistence;
using Connectied.Application.Repositories;
using Connectied.Domain;
using Connectied.Infrastructure.Persistence;
using Connectied.Infrastructure.Persistence.Interceptors;
using Connectied.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Connectied.Infrastructure;
public static class ConfigureServices
{
    static readonly ILogger Logger = Log.ForContext(typeof(ConfigureServices));

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddOptions<DatabaseOptions>()
            .BindConfiguration(nameof(DatabaseOptions))
            .ValidateOnStart()
            .PostConfigure(
                config =>
                {
                    Logger.Information("Current Database Provider: {DatabaseProvider}", config.Provider);
                    Logger.Information("Current Connection String: {ConnectionString}", config.ConnectionString);
                });
        services.AddDbContext<ConnectiedDbContext>(
            (sp, opts) =>
            {
                opts.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                var dbOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
                if (string.IsNullOrWhiteSpace(dbOptions.Provider))
                {
                    throw new InvalidOperationException("Database provider is not configured.");
                }
                if (string.IsNullOrWhiteSpace(dbOptions.ConnectionString))
                {
                    throw new InvalidOperationException("Database connection string is not configured.");
                }
                opts.UseSqlServer(dbOptions.ConnectionString);
            });

        var seederTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(ISeeder).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);
        foreach (var seederType in seederTypes)
        {
            services.AddTransient(typeof(ISeeder), seederType);
        }

        services.TryAddScoped<IDatabaseInitializer, DatabaseInitializer>();
        services.TryAddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        foreach (var aggregate in typeof(IAggregateRoot).Assembly
            .GetExportedTypes()
            .Where(t => typeof(IAggregateRoot).IsAssignableFrom(t) && t.IsClass)
            .ToImmutableList())
        {
            services.TryAddScoped(
                typeof(IReadRepository<>).MakeGenericType(aggregate),
                sp => sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregate)));
        }

        return services;
    }
}
