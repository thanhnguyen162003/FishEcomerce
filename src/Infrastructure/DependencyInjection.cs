using FishEcomerce.Application.Common.Interfaces;
using FishEcomerce.Infrastructure.Context;
using FishEcomerce.Infrastructure.Data.Interceptors;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        
        services.AddScoped<IKingFishDbContext>(provider => provider.GetRequiredService<KingFishDbContext>());
        
        services.AddSingleton(TimeProvider.System);
        
        return services;
    }
}
