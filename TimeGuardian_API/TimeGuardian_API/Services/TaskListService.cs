using AutoMapper;

using Microsoft.AspNetCore.Authorization;

using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;

namespace TimeGuardian_API.Services;

public interface ITaskListService
{
    TaskList Create(int userId);
    int GetFirstListId(int userId);
}

public class TaskListService : ITaskListService
{
    private readonly ApiDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;

    public TaskListService(ApiDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _authorizationService = authorizationService;
    }

    public int GetFirstListId(int userId)
    {
        var taskList = _dbContext.TaskLists.Where(x => x.UserId == userId).FirstOrDefault();

        if (taskList is null)
            taskList = Create(userId);

        return taskList.Id;
    }

    public TaskList Create(int userId)
    {
        var taskList = new TaskList()
        {
            CreationDate = DateTime.Now,
            Description = "",
            Name = "Default",
            UserId = userId,
        };

        _dbContext.TaskLists.Add(taskList);
        _dbContext.SaveChanges();

        taskList = _dbContext.TaskLists.Where(x => x.UserId == userId).FirstOrDefault();

        return taskList;
    }
}
