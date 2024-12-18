﻿using Application;
using Carter;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // Đảm bảo thư viện OpenAPI đã được thêm
using System.Text;
using Application.Common.ThirdPartyManager.Cloudinary;
using Application.Common.ThirdPartyManager.PayOS;
using Domain.Constants;
using Web;
using Web.Endpoints;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Nạp cấu hình từ appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

// Đọc SecretKey từ appsettings
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("SecretKey");

// Đăng ký các dịch vụ vào container
builder.Services.AddControllers();
builder.Services.AddCarter();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

// Default Password
builder.Services.Configure<DefaultPassword>(builder.Configuration.GetSection("DefaultPassword"));

// PayOS
builder.Services.Configure<PayOSSettings>(builder.Configuration.GetSection("PayOS"));

// Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Location", "X-Pagination");
        });
});

// Đăng ký Authentication với JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero 
    };
});

// Chỉ thêm SwaggerDoc một lần
builder.Services.AddSwaggerGen(options =>
{
    // Kiểm tra và chỉ thêm `SwaggerDoc` một lần duy nhất
    if (!options.SwaggerGeneratorOptions.SwaggerDocs.ContainsKey("v1"))
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "KingFish API",
            Version = "v1"
        });
    }

    

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Customer", policy =>
        policy.RequireRole("Customer"));
    options.AddPolicy("Staff", policy =>
        policy.RequireRole("Staff"));
    options.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin"));
    options.AddPolicy("Admin&Staff", policy =>
        policy.RequireRole("Admin", "Staff"));
});


var app = builder.Build();


app.UseHsts();
// Sử dụng các middleware cho Authentication và Authorization
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapAuthEndpoints();
app.UseSwagger();
app.UseSwaggerUI();
app.MapCarter();
app.MapControllers();

app.Run();
