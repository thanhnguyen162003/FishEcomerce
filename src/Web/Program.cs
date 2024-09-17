var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();
app.UseHsts();
// app.UseHealthChecks("/health");

// Swagger
app.UseSwagger();
app.UseSwaggerUI();
//

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseExceptionHandler(options => { });
app.Run();

public partial class Program
{
}
