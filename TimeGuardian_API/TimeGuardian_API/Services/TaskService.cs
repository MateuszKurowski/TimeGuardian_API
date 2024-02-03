using AutoMapper;

using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

using TimeGuardian_API.Authorization;

using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Exceptions;
using TimeGuardian_API.Models.Task;

using Task = TimeGuardian_API.Entities.Task;

namespace TimeGuardian_API.Services;

public interface ITaskService
{
    TaskDto Create(CreateByAdminTaskDto dto);
    TaskDto Create(CreateTaskDto dto, ClaimsPrincipal user);
    void Delete(int id, ClaimsPrincipal user);
    IEnumerable<TaskDto> GetAll();
    TaskDto GetById(int id, ClaimsPrincipal user);
    IEnumerable<TaskDto> GetByUserId(ClaimsPrincipal user);
    IEnumerable<TaskDto> GetByUserId(int userId);
    TaskDto Patch(CreateTaskDto dto, int id, ClaimsPrincipal user);
    TaskDto Update(CreateTaskDto dto, int id, ClaimsPrincipal user);
}

public class TaskService : ITaskService
{
    private readonly ApiDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;
    private readonly ITaskListService _taskListService;

    public TaskService(ApiDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService, ITaskListService taskListService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _authorizationService = authorizationService;
        _taskListService = taskListService;
    }

    public TaskDto GetById(int id, ClaimsPrincipal user)
    {
        var task = _dbContext
                                    .Tasks
                                    .FirstOrDefault(t => t.Id == id)
                                    ?? throw new NotFoundException(NotFoundException.Entities.Task);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, task, requirement: new TaskSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        return _mapper.Map<TaskDto>(task);
    }

    public IEnumerable<TaskDto> GetByUserId(ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        var tasks = _dbContext.Tasks?.Where(task => task.UserId == userId)?.Select(task => _mapper.Map<TaskDto>(task));

        return tasks is null
            ? Enumerable.Empty<TaskDto>()
            : tasks;
    }

    public IEnumerable<TaskDto> GetByUserId(int userId)
    {
        var task = _dbContext.Tasks?.Where(ta => ta.UserId == userId)?.Select(ta => _mapper.Map<TaskDto>(ta));

        return task is null
            ? Enumerable.Empty<TaskDto>()
            : task;
    }

    public IEnumerable<TaskDto> GetAll()
    {
        var tasks = _dbContext.Tasks?.Select(task => _mapper.Map<TaskDto>(task));

        return tasks is null
            ? Enumerable.Empty<TaskDto>()
            : tasks;
    }

    public TaskDto Update(CreateTaskDto dto, int id, ClaimsPrincipal user)
    {
        var task = _dbContext
                                     .Tasks
                                     .FirstOrDefault(p => p.Id == id)
                                     ?? throw new NotFoundException(NotFoundException.Entities.Task);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, task, requirement: new TaskSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        if (dto.DueDate is null || dto.Description is null || dto.Title is null || dto.IsCompleted is null || dto.CreateDate is null)
            throw new BadRequestException();

        task.DueDate = (DateTime)dto.DueDate;
        task.CreateDate = (DateTime)dto.CreateDate;
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = (bool)dto.IsCompleted;

        _dbContext.SaveChanges();

        return _mapper.Map<TaskDto>(task);
    }

    public TaskDto Patch(CreateTaskDto dto, int id, ClaimsPrincipal user)
    {
        var task = _dbContext
                                     .Tasks
                                     .FirstOrDefault(t => t.Id == id)
                                     ?? throw new NotFoundException(NotFoundException.Entities.Task);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, task, requirement: new TaskSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        var wasChanges = false;
        if (dto.DueDate != null)
        {
            task.DueDate = (DateTime)dto.DueDate;
            wasChanges = true;
        }
        if (dto.CreateDate != null)
        {
            task.CreateDate = (DateTime)dto.CreateDate;
            wasChanges = true;
        }
        if (string.IsNullOrWhiteSpace(dto.Title) is false)
        {
            task.Title = dto.Title;
            wasChanges = true;
        }
        if (string.IsNullOrWhiteSpace(dto.Description) is false)
        {
            task.Description = dto.Description;
            wasChanges = true;
        }
        if (dto.IsCompleted != null)
        {
            task.IsCompleted = (bool)dto.IsCompleted;
            wasChanges = true;
        }

        if (wasChanges)
            _dbContext.SaveChanges();

        return _mapper.Map<TaskDto>(task);
    }

    public TaskDto Create(CreateTaskDto dto, ClaimsPrincipal user)
    {
        var task = _mapper.Map<Task>(dto);
        var userIds = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var userId = int.Parse(userIds);
        task.UserId = userId;
        var taskListId =  _taskListService.GetFirstListId(userId);
        task.TaskListId = taskListId;

        var userDAO = _dbContext.Users.FirstOrDefault(x => x.Id == task.UserId)
            ?? throw new NotFoundException(NotFoundException.Entities.Task);
        task.User = userDAO;

        _dbContext.Tasks.Add(task);
        _dbContext.SaveChanges();

        return _mapper.Map<TaskDto>(task);
    }

    public TaskDto Create(CreateByAdminTaskDto dto)
    {
        var task = _mapper.Map<Task>(dto);
        task.UserId = dto.UserId;
        var taskListId = _taskListService.GetFirstListId(dto.UserId);
        task.TaskListId = taskListId;

        _dbContext.Tasks.Add(task);

        var userDAO = _dbContext.Users.FirstOrDefault(x => x.Id == task.UserId)
            ?? throw new NotFoundException(NotFoundException.Entities.Task);
        task.User = userDAO;

        _dbContext.SaveChanges();

        return _mapper.Map<TaskDto>(task);
    }

    public void Delete(int id, ClaimsPrincipal user)
    {
        var task = _dbContext
                        .Tasks
                        .FirstOrDefault(t => t.Id == id)
        ?? throw new NotFoundException(NotFoundException.Entities.Task);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, task, requirement: new TaskSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        _dbContext.Tasks.Remove(task);
        _dbContext.SaveChanges();
    }
}
