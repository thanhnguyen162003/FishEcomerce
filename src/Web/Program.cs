using Application;
using Carter;
using Infrastructure;
using Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCarter();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();

app.UseHsts();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapCarter();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.Run();