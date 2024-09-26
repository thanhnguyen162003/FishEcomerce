using FishEcomerce.Domain.Interfaces;
using FishEcomerce.Infrastructure.Context;
using FishEcomerce.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        
        services.AddDbContext<KingFishContext>((serviceProvider, optionBuilder) =>
        {
            optionBuilder.UseNpgsql(connectionString);
            var interceptor = serviceProvider.GetRequiredService<AuditableEntityInterceptor>();
            optionBuilder.AddInterceptors(interceptor);
        });
        
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Repo
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IProductRepository, ProductRepository>();
        
        return services;
    }
}
