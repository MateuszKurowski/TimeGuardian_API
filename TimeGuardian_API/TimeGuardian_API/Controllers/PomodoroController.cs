using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TimeGuardian_API.Models.Pomodoro;
using TimeGuardian_API.Models.Task;
using TimeGuardian_API.Services;

namespace TimeGuardian_API.Controllers;

[Authorize]
[ApiController]
[Route("api/pomodoro")]
public class PomodoroController : ControllerBase
{
    private readonly IPomodoroService _pomodoroService;

    public PomodoroController(IPomodoroService pomodoroService)
    {
        _pomodoroService = pomodoroService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public ActionResult<IEnumerable<PomodoroDto>> GetAll()
        => Ok(_pomodoroService.GetAll());

    [HttpGet("{id}")]
    public ActionResult<PomodoroDto> Get([FromRoute] int id)
    {
        var pomodoro = _pomodoroService.GetById(id, User);
        return Ok(pomodoro);
    }

    [Authorize(Roles = "Admin")]
    [Route("user/{userId}")]
    [HttpGet]
    public ActionResult<IEnumerable<PomodoroDto>> GetByUserId([FromRoute] int userId)
    {
        var pomodoro = _pomodoroService.GetByUserId(userId);
        return Ok(pomodoro);
    }

    [Route("account")]
    [HttpGet]
    public ActionResult<IEnumerable<PomodoroDto>> GetByAccount()
    {
        var pomodoro = _pomodoroService.GetByUserId(User);
        return Ok(pomodoro);
    }

    [HttpPut("{id}")]
    public ActionResult<PomodoroDto> Update([FromBody] CreatePomodoroDto dto, [FromRoute] int id)
    {
        var pomodoro = _pomodoroService.Update(dto, id, User);
        return Ok(pomodoro);
    }

    [HttpPatch("{id}")]
    public ActionResult<TaskDto> Patch([FromBody] CreatePomodoroDto dto, [FromRoute] int id)
    {
        var task = _pomodoroService.Patch(dto, id, User);
        return Ok(task);
    }

    [HttpPost]
    public ActionResult Create([FromBody] CreateByAdminPomodoroDto pomodoroDto)
    {
        var pomodoro = _pomodoroService.Create(pomodoroDto);
        return Created($"/api/pomodoro/{pomodoro.Id}", pomodoro);
    }

    [HttpPost]
    [Route("account")]
    public ActionResult Create([FromBody] CreatePomodoroDto dto)
    {
        var pomodoro = _pomodoroService.Create(dto, User);
        return Created($"/api/pomodoro/{pomodoro.Id}", pomodoro);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        _pomodoroService.Delete(id, User);
        return NoContent();
    }
}
