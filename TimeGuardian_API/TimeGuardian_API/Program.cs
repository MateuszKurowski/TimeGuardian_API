using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using NLog;
using NLog.Web;

using System.Reflection;

using TimeGuardian_API.Data;
using TimeGuardian_API.Middleware;
using TimeGuardian_API.Services;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json");
    LogManager.Setup().LoadConfigurationFromFile("nlog.Development.config");
}
else
{
    builder.Configuration.AddJsonFile("appsettings.Production.json");
    LogManager.Setup().LoadConfigurationFromFile("nlog.Production.config");
}

builder.Services.AddControllers();

ApiDbContext.ApplyDbContext(builder);
builder.Services.AddScoped<DbSeeder>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISessionTypeService, SessionTypeService>();

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TimeGuardian API", Version = "v1" });
});

/* build */

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApiDbContext>();

    if (context.Database.GetPendingMigrations().Any())
        context.Database.Migrate();

    var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
    seeder.Seed();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TimeGuardian API"));
}

app.UseRouting();
app.MapControllers();
app.Run();