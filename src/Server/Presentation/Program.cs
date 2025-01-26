using Application.DI;
using Infrastructure.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddInfrastructureDependencies(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddApplicationDependencies(builder.Configuration);
    
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

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
