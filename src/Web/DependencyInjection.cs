﻿using Application.Auth;
using Application.Breeds.Commands.CreateBreed;
using Application.Breeds.Commands.UpdateBreed;
using Application.Common.Models.BreedModels;
using Application.Common.Models.FishAwardModels;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Application.FishAwards.Commands.CreateFishAward;
using Application.FishAwards.Commands.UpdateFishAward;
using Application.Products.Commands.CreateFishProduct;
using Application.Products.Commands.UpdateFishProduct;
using FluentValidation;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        //Auth
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        

        services.AddScoped(typeof(ValidationHelper<>));

        services.AddHttpContextAccessor();

        // services.AddHealthChecks()
        //     .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();
        
        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Aquamarine API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
        
        return services;
    }
}