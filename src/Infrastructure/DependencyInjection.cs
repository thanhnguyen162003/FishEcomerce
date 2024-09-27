using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");


        services.AddDbContext<KingFishDbContext>((optionBuilder) =>
        {
            optionBuilder.UseNpgsql(connectionString);
        });

        services.AddSingleton(TimeProvider.System);

        // Repo
        // services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        // services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}