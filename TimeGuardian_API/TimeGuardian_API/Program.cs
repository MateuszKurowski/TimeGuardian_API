﻿using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using NLog;
using NLog.Web;

using System.Reflection;
using System.Text;

using TimeGuardian_API;
using TimeGuardian_API.Authorization;
using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Middleware;
using TimeGuardian_API.Models;
using TimeGuardian_API.Models.Login;
using TimeGuardian_API.Models.Role;
using TimeGuardian_API.Models.Session;
using TimeGuardian_API.Models.SessionType;
using TimeGuardian_API.Models.User;
using TimeGuardian_API.Services;
using TimeGuardian_API.Validators;

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

var autheticationSetting = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(autheticationSetting);
builder.Services.AddSingleton(autheticationSetting);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;

    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = autheticationSetting.JwtIssuer,
        ValidAudience = autheticationSetting.JwtIssuer,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(autheticationSetting.JwtKey)),
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SelfRequirment", build => build.AddRequirements(new SelfRequirement()));
});
builder.Services.AddScoped<IAuthorizationHandler, SelfRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, SessionSelfRequirmentHandler>();
builder.Services.AddScoped<IAuthorizationHandler, SessionTypeSelfRequirmentHandler>();
builder.Services.AddScoped<IAuthorizationHandler, PomodoroSelfRequirmentHandler>();
builder.Services.AddScoped<IAuthorizationHandler, TaskSelfRequirmentHandler>();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

ApiDbContext.ApplyDbContext(builder);
builder.Services.AddScoped<DbSeeder>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IUtilityService, UtilityService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISessionTypeService, SessionTypeService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IPomodoroService, PomodoroService>();
builder.Services.AddScoped<ITaskListService, TaskListService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
builder.Services.AddScoped<IValidator<StartSessionDto>, StartSessionDtoValidator>();
builder.Services.AddScoped<IValidator<EndSessionDto>, EndSessionDtoValidator>();
builder.Services.AddScoped<IValidator<CreateSessionDto>, CreateSessionDtoValidator>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
builder.Services.AddScoped<IValidator<PasswordDto>, PasswordDtoValidator>();
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
            .AllowAnyOrigin()
            .AllowAnyHeader()));

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
app.UseAuthentication();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TimeGuardian API"));
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.Run();