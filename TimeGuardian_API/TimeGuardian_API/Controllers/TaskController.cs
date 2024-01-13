using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using TimeGuardian_API.Entities;
using TimeGuardian_API.Models.Task;
using TimeGuardian_API.Services;

namespace TimeGuardian_API.Controllers;

[Authorize]
[ApiController]
[Route("api/task")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ITaskListService _taskListService;

    public TaskController(ITaskService taskService, ITaskListService taskListService)
    {
        _taskService = taskService;
        _taskListService = taskListService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public ActionResult<IEnumerable<TaskDto>> GetAll()
        => Ok(_taskService.GetAll());

    [HttpGet("{id}")]
    public ActionResult<TaskDto> Get([FromRoute] int id)
    {
        var task = _taskService.GetById(id, User);
        return Ok(task);
    }

    [Authorize(Roles = "Admin")]
    [Route("user/{userId}")]
    [HttpGet]
    public ActionResult<IEnumerable<TaskDto>> GetByUserId([FromRoute] int userId)
    {
        var task = _taskService.GetByUserId(userId);
        return Ok(task);
    }

    [Route("account")]
    [HttpGet]
    public ActionResult<IEnumerable<TaskDto>> GetByAccount()
    {
        var task = _taskService.GetByUserId(User);
        return Ok(task);
    }

    [HttpPut("{id}")]
    public ActionResult<TaskDto> Update([FromBody] CreateTaskDto dto, [FromRoute] int id)
    {
        var task = _taskService.Update(dto, id, User);
        return Ok(task);
    }

    [HttpPatch("{id}")]
    public ActionResult<TaskDto> Patch([FromBody] CreateTaskDto dto, [FromRoute] int id)
    {
        var task = _taskService.Patch(dto, id, User);
        return Ok(task);
    }

    [HttpPost]
    public ActionResult Create([FromBody] CreateByAdminTaskDto dto)
    {
        var task = _taskService.Create(dto);
        return Created($"/api/task/{task.Id}", task);
    }

    [HttpPost]
    [Route("account")]
    public ActionResult Create([FromBody] CreateTaskDto dto)
    {
        var task = _taskService.Create(dto, User);
        return Created($"/api/task/{task.Id}", task);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        _taskService.Delete(id, User);
        return NoContent();
    }
}
