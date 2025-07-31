using Application.DI;
using Infrastructure.DI;
using Microsoft.OpenApi.Models;
using Presentation.DI;
using Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Configure(context.Configuration.GetSection("Kestrel"));
});

// Add services
builder.Services
    .AddApplicationDependencies(builder.Configuration)
    .AddInfrastructureDependencies(builder.Configuration)
    .AddPresentationDependencies(builder.Configuration);

builder.Services.AddTransient<ExceptionHandlerMiddleware>();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularLocalhost",
        corsPolicyBuilder => corsPolicyBuilder
            .WithOrigins("http://localhost:4200", "https://localhost:4200", "http://localhost:80")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
        );
});

var app = builder.Build();

app.UseCors("AllowAngularLocalhost");

app.ApplyDatabaseMigration();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
