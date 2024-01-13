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

            if (!_dbContext.TaskLists.Any())
            {
                var taskLists = GetTaskLists();
                _dbContext.TaskLists.AddRange(taskLists);
                wasChanges = true;
            }

            if (!_dbContext.Tasks.Any())
            {
                var tasks = GetTasks();
                _dbContext.Tasks.AddRange(tasks);
                wasChanges = true;
            }

            if (!_dbContext.Pomodoro.Any())
            {
                var pomodoro = GetPomodoro();
                _dbContext.Pomodoro.AddRange(pomodoro);
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

    private IEnumerable<TaskList> GetTaskLists()
    {
        var lists = new List<TaskList>();
        var list1 = new TaskList()
        {
            Id = 1,
            Description = "",
            Name = "Default",
            UserId = 1,
        };
        lists.Add(list1);

        var list2 = new TaskList()
        {
            Id = 2,
            Description = "",
            Name = "Default",
            UserId = 2,
        };
        lists.Add(list2);
        return lists;
    }

    private IEnumerable<Entities.Task> GetTasks()
    {
        var tasks = new List<Entities.Task>();
        var task1 = new Entities.Task()
        {
            Id = 1,
            Title = "Zrobić zakupy spożywcze",
            Description = "Kupić mleko, chleb, jajka, owoce",
            IsCompleted = false,
            CreateDate = DateTime.Now,
            TaskListId = 1,
            UserId = 1,
            DueDate = DateTime.Now.AddDays(2),
        };
        tasks.Add(task1);

        var task2 = new Entities.Task()
        {
            Id = 2,
            Title = "Naprawić wyciek w kranie",
            Description = "Użyć klucza do naprawy przeciekającego kranu w łazience",
            IsCompleted = true,
            CreateDate = DateTime.Now,
            TaskListId = 2,
            UserId = 2,
            DueDate = DateTime.Now.AddDays(-3),
        };
        tasks.Add(task2);

        var task3 = new Entities.Task()
        {
            Id = 3,
            Title = "Przeczytać nową książkę",
            Description = "Rozpocząć czytanie 'Wichry niesamowitości' autorstwa Brandon Sandersona",
            IsCompleted = false,
            CreateDate = DateTime.Now,
            TaskListId = 2,
            UserId = 2,
            DueDate = DateTime.Now.AddDays(10),
        };
        tasks.Add(task3);

        var task4 = new Entities.Task()
        {
            Id = 4,
            Title = "Rozpocząć kurs online z programowania",
            Description = "Zarejestrować się na platformie edukacyjnej i rozpocząć naukę programowania w języku Python",
            IsCompleted = false,
            CreateDate = DateTime.Now,
            TaskListId = 2,
            UserId = 2,
            DueDate = DateTime.Now.AddDays(7),
        };
        tasks.Add(task4);

        var task5 = new Entities.Task()
        {
            Id = 5,
            Title = "Odwiedzić muzeum sztuki współczesnej",
            Description = "Zaplanować wizytę w Muzeum Sztuki Współczesnej i zwiedzić aktualne wystawy",
            IsCompleted = false,
            CreateDate = DateTime.Now,
            TaskListId = 1,
            UserId = 1,
            DueDate = DateTime.Now.AddDays(14),
        };
        tasks.Add(task5);

        var task6 = new Entities.Task()
        {
            Id = 6,
            Title = "Zorganizować spotkanie z przyjaciółmi",
            Description = "Zaplanować spotkanie w ulubionej kawiarni i spędzić czas z przyjaciółmi",
            IsCompleted = false,
            CreateDate = DateTime.Now,
            TaskListId = 1,
            UserId = 1,
            DueDate = DateTime.Now.AddDays(7),
        };
        tasks.Add(task6);

        var task7 = new Entities.Task()
        {
            Id = 7,
            Title = "Udekorować pokój na święta",
            Description = "Kupić ozdoby świąteczne i udekorować pokój przed nadchodzącymi świętami",
            IsCompleted = true,
            CreateDate = DateTime.Now,
            TaskListId = 2,
            UserId = 2,
            DueDate = DateTime.Now.AddDays(10),
        };
        tasks.Add(task7);

        var task8 = new Entities.Task()
        {
            Id = 8,
            Title = "Pójść na zajęcia jogi",
            Description = "",
            IsCompleted = false,
            CreateDate = DateTime.Now,
            TaskListId = 2,
            UserId = 2,
            DueDate = DateTime.Now.AddDays(-5),
        };
        tasks.Add(task8);

        var task9 = new Entities.Task()
        {
            Id = 9,
            Title = "Napisać list do przyjaciela",
            Description = "Przypomnieć się przyjacielowi i podzielić się aktualnymi wydarzeniami",
            IsCompleted = false,
            CreateDate = DateTime.Now,
            TaskListId = 1,
            UserId = 1,
            DueDate = DateTime.Now.AddDays(5),
        };
        tasks.Add(task9);

        var task10 = new Entities.Task()
        {
            Id = 10,
            Title = "Sprzątnąć dom",
            Description = "Zorganizować generalne porządki w domu i pozbyć się zbędnych rzeczy",
            IsCompleted = false,
            CreateDate = DateTime.Now,
            TaskListId = 1,
            UserId = 1,
            DueDate = DateTime.Now.AddDays(7),
        };
        tasks.Add(task10);

        var task11 = new Entities.Task()
        {
            Id = 1,
            Title = "Wymienić olej w samochodzie",
            Description = "",
            IsCompleted = false,
            CreateDate = DateTime.Now,
            TaskListId = 1,
            UserId = 1,
            DueDate = DateTime.Now.AddDays(-10),
        };
        tasks.Add(task1);

        var task12 = new Entities.Task()
        {
            Id = 1,
            Title = "Umówić się do fryzjera",
            Description = "Najlepiej do Pana Andrzej bo najlepiej wie jak mnie ostrzyc",
            IsCompleted = true,
            CreateDate = DateTime.Now,
            TaskListId = 1,
            UserId = 1,
            DueDate = DateTime.Now.AddDays(-4),
        };
        tasks.Add(task2);

        return tasks;
    }

    private IEnumerable<Pomodoro> GetPomodoro()
    {
        var pomodoros = new List<Pomodoro>();
        var pomodoro1 = new Pomodoro()
        {
            Id = 1,
            UserId = 1,
            Date = DateTime.Now.AddDays(-5),
            Duration = 30
        };
        pomodoros.Add(pomodoro1);

        var pomodoro2 = new Pomodoro()
        {
            Id = 2,
            UserId = 1,
            Date = DateTime.Now.AddDays(-3),
            Duration = 10
        };
        pomodoros.Add(pomodoro2);

        var pomodoro3 = new Pomodoro()
        {
            Id = 3,
            UserId = 1,
            Date = DateTime.Now.AddDays(-1),
            Duration = 30
        };
        pomodoros.Add(pomodoro3);

        var pomodoro4 = new Pomodoro()
        {
            Id = 4,
            UserId = 2,
            Date = DateTime.Now.AddDays(-5),
            Duration = 30
        };
        pomodoros.Add(pomodoro4);

        var pomodoro5 = new Pomodoro()
        {
            Id = 5,
            UserId = 2,
            Date = DateTime.Now.AddDays(-3),
            Duration = 10
        };
        pomodoros.Add(pomodoro5);

        var pomodoro6 = new Pomodoro()
        {
            Id = 6,
            UserId = 2,
            Date = DateTime.Now.AddDays(-1),
            Duration = 30
        };
        pomodoros.Add(pomodoro6);

        return pomodoros;
    }
}