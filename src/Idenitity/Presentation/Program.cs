using Application.DI;
using Infrastructure.DI;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;
using Presentation.DI;
using Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Configure(context.Configuration.GetSection("Kestrel"));
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

builder.Services.AddTransient<ExceptionHandlerMiddleware>();

builder.Services
    .AddPresentationDependencies(builder.Configuration)
    .AddInfrastructureDependencies(builder.Configuration)
    .AddApplicationDependencies(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularLocalhost",
        corsPolicyBuilder => corsPolicyBuilder
            .WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
    );
});

var app = builder.Build();

app.UseCors("AllowAngularLocalhost");

app.ApplyDatabaseMigration();

app.MapControllers();
app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
