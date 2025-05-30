using Application.DI;
using Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Configure(context.Configuration.GetSection("Kestrel"));
});

// Add services
builder.Services
    .AddApplicationDependencies(builder.Configuration)
    .AddInfrastructureDependencies(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddApplicationDependencies(builder.Configuration);
    
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.ApplyDatabaseMigration();

// TODO: Remove true for production
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
