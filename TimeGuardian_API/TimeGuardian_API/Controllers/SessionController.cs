using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TimeGuardian_API.Models.Session;
using TimeGuardian_API.Services;

namespace TimeGuardian_API.Controllers;

[Authorize]
[Route("api/session")]
[ApiController]
public class SessionsController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<SessionDto>> GetAll()
        => Ok(_sessionService.GetAll());

    [HttpGet("{id}")]
    public ActionResult<SessionDto> Get([FromRoute] int id)
    {
        var sessionType = _sessionService.GetById(id);
        return Ok(sessionType);
    }

    [HttpPut("{id}")]
    public ActionResult<SessionDto> Update([FromBody] CreateSessionDto dto, [FromRoute] int id)
    {
        var sessionType = _sessionService.Update(dto, id);
        return Ok(sessionType);
    }

    [HttpPatch("{id}")]
    public ActionResult<SessionDto> Patch([FromBody] PatchSessionDto dto, [FromRoute] int id)
    {
        var sessionType = _sessionService.Patch(dto, id);
        return Ok(sessionType);
    }

    [HttpPost]
    public ActionResult Create([FromBody] CreateSessionDto dto)
    {
        var id = _sessionService.Create(dto);
        return Created($"/api/session/{id}", null);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        _sessionService.Delete(id);
        return NoContent();
    }

    [HttpPost]
    [Route("start")]
    public ActionResult StartSession([FromBody] StartSessionDto dto)
    {
        var id = _sessionService.StartSession(dto);
        return Created($"/api/session/{id}", null);
    }


    [HttpPost]
    [Route("end")]
    public ActionResult EndSession([FromBody] EndSessionDto dto)
    {
        var id = _sessionService.EndSession(dto);
        return Created($"/api/session/{id}", null);
    }
}