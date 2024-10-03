using System.Configuration;
using System.Reflection;
using Application.Auth;
using Application.Common.Models.BreedModels;
using Application.Common.ThirdPartyManager.Cloudinary;
using Application.Common.UoW;
using Application.Common.Utils;
using FluentValidation;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped(typeof(ValidationHelper<>));
        
        services.AddScoped<IClaimsService, ClaimsService>();
        
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        //Auth
        services.AddScoped<IAuthService, AuthService>();
        
        


        return services;
    }
}