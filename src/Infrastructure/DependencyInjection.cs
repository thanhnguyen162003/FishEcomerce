// using FishEcomerce.Application.Common.Interfaces;
// using FishEcomerce.Infrastructure.Context;

using FishEcomerce.Application.Common.Interfaces;
using FishEcomerce.Infrastructure.Context;
using FishEcomerce.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        
        services.AddDbContext<KingFishContext>(options =>
        {
            if (connectionString != null)
            {
                options.UseNpgsql(configuration.GetConnectionString(connectionString),
                    b => b.MigrationsAssembly(typeof(KingFishContext).Assembly.FullName));
            }
        }, ServiceLifetime.Transient);
        
        services.AddScoped<IKingFishDbContext>(provider => provider.GetRequiredService<KingFishContext>());
        
        services.AddSingleton(TimeProvider.System);
        
        return services;
    }
}
