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

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

builder.Services.AddTransient<ExceptionHandlerMiddleware>();

builder.Services
    .AddPresentationDependencies(builder.Configuration)
    .AddInfrastructureDependencies(builder.Configuration)
    .AddApplicationDependencies(builder.Configuration);

var app = builder.Build();

app.ApplyDatabaseMigration();

app.MapControllers();
app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
