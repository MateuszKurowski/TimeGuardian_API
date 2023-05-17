using Microsoft.EntityFrameworkCore;

using TimeGuardian_API.Models;

namespace TimeGuardian_API.Data;

public class ApiContext : DbContext
{
    public DbSet<Session> Sessions { get; set; }
    public DbSet<SessionType> SessionTypes { get; set; }
    public DbSet<User> Users { get; set; }

    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SessionType>().HasData(
                new SessionType { Id = 1, Name = "Nauka" },
                new SessionType { Id = 2, Name = "Praca" },
                new SessionType { Id = 3, Name = "Prokrastynacja" }
            );

        modelBuilder.Entity<User>().HasData(
               new User
               {
                   Id = 1,
                   Login = "Admin",
                   Password = "Admin1",
                   Email = "admin@admin.com",
                   CreatedAt = DateTime.Now
               }
            );
    }
}