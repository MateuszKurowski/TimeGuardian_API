using Microsoft.AspNetCore.Identity;

using TimeGuardian_API.Entities;

namespace TimeGuardian_API.Data;

public class DbSeeder
{
    private readonly ApiDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public DbSeeder(ApiDbContext dbContext, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public void Seed()
    {
        if (_dbContext.Database.CanConnect())
        {
            var wasChanges = false;
            if (!_dbContext.Roles.Any())
            {
                var roles = GetRoles();
                _dbContext.Roles.AddRange(roles);
                wasChanges = true;
            }

            if (!_dbContext.SessionTypes.Any())
            {
                var sessionTypes = GetSessionTypes();
                _dbContext.SessionTypes.AddRange(sessionTypes);
                wasChanges = true;
            }

            if (!_dbContext.Users.Any())
            {
                var users = GetUsers();
                _dbContext.Users.AddRange(users);
                wasChanges = true;
            }

            if (!_dbContext.Sessions.Any())
            {
                var sessions = GetSessions();
                _dbContext.Sessions.AddRange(sessions);
                wasChanges = true;
            }

            if (wasChanges)
                _dbContext.SaveChanges();
        }
    }

    private IEnumerable<Session> GetSessions()
        => new List<Session>()
            {
                new Session
                {
                    Id = 1,
                    UserId = 1,
                    SessionTypeId = 1,
                    StartTime = new DateTime(2023, 5, 17, 17, 12, 34),
                    EndTime = new DateTime(2023, 5, 17, 18, 58, 13),
                    Duration = 6339,
                    Deleted = false
                },

                new Session
                {
                    Id = 2,
                    UserId = 1,
                    SessionTypeId = 3,
                    StartTime = new DateTime(2023, 5, 17, 18, 58, 13),
                    EndTime = new DateTime(2023, 5, 17, 19, 58, 13),
                    Duration = 3600,
                    Deleted = false
                },

                new Session
                {
                    Id = 3,
                    UserId = 1,
                    SessionTypeId = 2,
                    StartTime = new DateTime(2023, 5, 17, 19, 23, 56),
                    EndTime = new DateTime(2023, 5, 17, 23, 47, 3),
                    Duration = 15787,
                    Deleted = false
                },

                new Session
                {
                    Id = 4,
                    UserId = 2,
                    SessionTypeId = 1,
                    StartTime = new DateTime(2023, 5, 17, 19, 23, 56),
                    EndTime = new DateTime(2023, 5, 17, 23, 47, 3),
                    Duration = 15787,
                    Deleted = false
                }
            };

    private IEnumerable<Role> GetRoles()
        => new List<Role>()
            {
                new Role()
                {
                    Id = 1,
                    Name = "Admin"
                },

                new Role()
                {
                    Id = 2,
                    Name = "User"
                }
            };

    private IEnumerable<SessionType> GetSessionTypes()
        => new List<SessionType>()
            {
                new SessionType()
                {
                    Id = 1,
                    Name = "Nauka",
                    CreatedById = null,
                    Default = true
                },

                new SessionType()
                {
                    Id = 2,
                    Name = "Praca",
                    CreatedById = null,
                    Default = true
                },

                new SessionType()
                {
                    Id = 3,
                    Name = "Prokrastynacja",
                    CreatedById = null,
                    Default = true
                }
            };

    private IEnumerable<User> GetUsers()
    {
        var users = new List<User>();
        var user1 = new User()
        {
            Id = 1,
            Email = "admin@admin.pl",
            FirstName = "Admin",
            LastName = "Nimda",
            DateOfBirth = new DateTime(1998, 05, 09),
            CreatedAt = DateTime.Now,
            Nationality = "Poland",
            RoleId = 1,
            Deleted = false
        };
        user1.PasswordHash = _passwordHasher.HashPassword(user1, "Admin1");
        users.Add(user1);

        var user2 = new User()
        {
            Id = 2,
            Email = "jan.nowak@gmail.pl",
            FirstName = "Jan",
            LastName = "Nowak",
            DateOfBirth = new DateTime(2001, 01, 30),
            CreatedAt = DateTime.Now,
            Nationality = "England",
            RoleId = 2,
            Deleted = false
        };
        user2.PasswordHash = _passwordHasher.HashPassword(user2, "Jan1");
        users.Add(user2);
        
        return users;
    }
}