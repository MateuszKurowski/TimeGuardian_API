using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using NLog;
using NLog.Web;

using System.Reflection;

using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Middleware;
using TimeGuardian_API.Models;
using TimeGuardian_API.Models.Role;
using TimeGuardian_API.Models.SessionType;
using TimeGuardian_API.Models.Validators;
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
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

ApiDbContext.ApplyDbContext(builder);
builder.Services.AddScoped<DbSeeder>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISessionTypeService, SessionTypeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
builder.Services.AddScoped<IValidator<CreateSessionTypeDto>, CreateSesstionTypeDtoValidator>();
builder.Services.AddScoped<IValidator<CreateRoleDto>, CreateRoleDtoValidator>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TimeGuardian API", Version = "v1" });
});

builder.Services.AddCors(options =>
    options.AddPolicy("FrontEndClient",
        policy =>
            policy.AllowAnyMethod()
            .AllowAnyOrigin()));

/* build */

var app = builder.Build();
app.UseCors("FrontEndClient");

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