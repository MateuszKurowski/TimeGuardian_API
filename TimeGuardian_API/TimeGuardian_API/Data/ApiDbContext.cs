using Microsoft.EntityFrameworkCore;

using TimeGuardian_API.Entities;

namespace TimeGuardian_API.Data;

public class ApiDbContext : DbContext
{
    public DbSet<Session> Sessions { get; set; }
    public DbSet<SessionType> SessionTypes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
                    .Property(e => e.Email)
                    .HasMaxLength(256)
                    .IsRequired();
        modelBuilder.Entity<User>()
                    .Property(e => e.FirstName)
                    .HasMaxLength(256);
        modelBuilder.Entity<User>()
                    .Property(e => e.LastName)
                    .HasMaxLength(256);
        modelBuilder.Entity<User>()
                    .Property(e => e.Nationality)
                    .HasMaxLength(64);
        modelBuilder.Entity<User>()
                    .Property(e => e.CreatedAt)
                    .IsRequired();

        modelBuilder.Entity<Role>()
                    .Property(e => e.Name)
                    .HasMaxLength(64)
                    .IsRequired();

        modelBuilder.Entity<SessionType>()
                    .Property(e => e.Name)
                    .HasMaxLength(64)
                    .IsRequired();

        modelBuilder.Entity<Session>()
                    .Property(e => e.StartTime)
                    .IsRequired();
        modelBuilder.Entity<Session>()
                    .Property(e => e.UserId)
                    .IsRequired();
        modelBuilder.Entity<Session>()
                    .Property(e => e.SessionTypeId)
                    .IsRequired();
    }

    public static void ApplyDbContext(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        if (builder.Environment.IsDevelopment())
            builder.Services.AddDbContextPool<ApiDbContext>(options
                                                            => options.UseMySql(connectionString,
                                                                ServerVersion.Create(
                                                                    new Version(10, 11, 3),
                                                                    Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MariaDb),
                                                                providerOpions
                                                                    => providerOpions.EnableRetryOnFailure())
                                                            .LogTo(Console.WriteLine, LogLevel.Trace));
        else
            builder.Services.AddDbContextPool<ApiDbContext>(options
                                                            => options.UseMySql(connectionString,
                                                                    ServerVersion.Create(
                                                                        new Version(10, 11, 3),
                                                                        Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MariaDb),
                                                                    providerOpions
                                                                        => providerOpions.EnableRetryOnFailure()));
    }
}