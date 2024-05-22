using Galaxon.Astronomy.Algorithms.Services;
using Galaxon.Astronomy.Data;
using Galaxon.Astronomy.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Galaxon.Tests.Astronomy;

public static class ServiceManager
{
    private static ServiceProvider? _serviceProvider;

    public static void Initialize()
    {
        _serviceProvider = new ServiceCollection()
            // Register DbContext.
            .AddDbContext<AstroDbContext>()
            // Register repositories.
            .AddScoped<AstroObjectGroupRepository>()
            .AddScoped<AstroObjectRepository>()
            // Register services.
            .AddScoped<PlanetService>()
            .AddScoped<SunService>()
            .AddScoped<LunarPhaseService>()
            .AddScoped<SeasonalMarkerService>()
            .AddScoped<ApsideService>()
            // Build it.
            .BuildServiceProvider();
    }

    public static T GetService<T>() where T : notnull
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("Service provider is uninitialised.");
        }

        return _serviceProvider.GetRequiredService<T>();
    }

    public static void Dispose()
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("Service provider is uninitialised.");
        }

        _serviceProvider.Dispose();
    }
}
