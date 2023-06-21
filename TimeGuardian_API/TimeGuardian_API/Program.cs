using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using System.Reflection;

using TimeGuardian_API.Data;
using TimeGuardian_API.Services;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
    builder.Configuration.AddJsonFile("appsettings.Development.json");
else 
    builder.Configuration.AddJsonFile("appsettings.Production.json");


//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//        .AddJwtBearer(options =>
//            {
//                options.RequireHttpsMetadata = false;
//                options.SaveToken = true;
//                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//                {
//                    ValidateIssuer = true,
//                    ValidateAudience = true,
//                    ValidateLifetime = true,
//                    ValidateIssuerSigningKey = true,

//                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                    ValidAudience = builder.Configuration["Jwt:Audience"],
//                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//                };
//            });

builder.Services.AddControllers();
ApiDbContext.ApplyDbContext(builder);
builder.Services.AddScoped<DbSeeder>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISessionTypeService, SessionTypeService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TimeGuardian - demonstracja", Version = "v0" });
 //   c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
 //   {
 //       Name = "Authorization",
 //       Type = SecuritySchemeType.ApiKey,
 //       Scheme = "Bearer",
 //       BearerFormat = "JWT",
 //       In = ParameterLocation.Header,
 //       Description = "JWT Authorization header using the Bearer scheme."
 //   });
 //   c.AddSecurityRequirement(new OpenApiSecurityRequirement
 //{
 //    {
 //          new OpenApiSecurityScheme
 //            {
 //                Reference = new OpenApiReference
 //                {
 //                    Type = ReferenceType.SecurityScheme,
 //                    Id = "Bearer"
 //                }
 //            },
 //            new string[] {}
 //    }
 //});
});

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWTAuthDemo v1"));
}

//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();