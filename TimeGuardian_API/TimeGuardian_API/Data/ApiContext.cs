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
               },
               new User
               {
                   Id = 2,
                   Login = "Jan",
                   Password = "Jan1",
                   Email = "jan@jan.com",
                   CreatedAt = DateTime.Now
               }
            );

        modelBuilder.Entity<Session>().HasData(
            new Session { Id = 1, UserId = 1, SessionTypeId = 1, StartTime = new DateTime(2023, 5, 17, 17, 12, 34), EndTime = new DateTime(2023, 5, 17, 18, 58, 13), Duration = 6339 },
            new Session { Id = 2, UserId = 1, SessionTypeId = 3, StartTime = new DateTime(2023, 5, 17, 18, 58, 13), EndTime = new DateTime(2023, 5, 17, 19, 58, 13), Duration = 3600 },
            new Session { Id = 3, UserId = 1, SessionTypeId = 2, StartTime = new DateTime(2023, 5, 17, 19, 23, 56), EndTime = new DateTime(2023, 5, 17, 23, 47, 3), Duration = 15787 },
            new Session { Id = 4, UserId = 2, SessionTypeId = 1, StartTime = new DateTime(2023, 5, 17, 19, 23, 56), EndTime = new DateTime(2023, 5, 17, 23, 47, 3), Duration = 15787 }
            );
    }
}