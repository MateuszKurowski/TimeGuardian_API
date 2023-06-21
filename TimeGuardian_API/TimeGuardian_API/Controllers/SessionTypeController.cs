using Microsoft.AspNetCore.Mvc;

using TimeGuardian_API.Entities;
using TimeGuardian_API.Models;
using TimeGuardian_API.Services;

namespace TimeGuardian_API.Controllers;

[Route("api/sessiontype")]
[ApiController]
public class SessionTypeController : ControllerBase
{
    private readonly ISessionTypeService _sessionTypeService;

    public SessionTypeController(ISessionTypeService sessionTypeService)
        => _sessionTypeService = sessionTypeService;

    [HttpGet]
    public ActionResult<IEnumerable<SessionType>> GetAll()
        => Ok(_sessionTypeService.GetAll());

    [HttpGet("{id}")]
    public ActionResult<SessionType> Get([FromRoute] int id)
    {
        var sessionType = _sessionTypeService.GetById(id);

        if (sessionType == null)
            return NotFound();

        return Ok(sessionType);
    }

    [HttpPut("{id}")]
    public ActionResult<SessionType> Put(int id, SessionTypeDto sessionTypeDto)
    {
        var existingSessionType = _sessionTypeService.GetByName(sessionTypeDto.Name);
        if (existingSessionType != null)
            return Conflict(new { message = $"SessionType with name {sessionTypeDto.Name} already exists." });

        var sessionType = _sessionTypeService.Update(id, sessionTypeDto);
        if (sessionType is null)
            return NotFound();

        return Ok(sessionType);
    }

    [HttpPost]
    public ActionResult Create(SessionTypeDto sessionTypeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingSessionType = _sessionTypeService.GetByName(sessionTypeDto.Name);
        if (existingSessionType != null)
            return Conflict(new { message = $"SessionType with name {sessionTypeDto.Name} already exists." });

        var id = _sessionTypeService.Create(sessionTypeDto);

        return Created($"/api/sessiontype/{id}", null);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var isDeleted = _sessionTypeService.Delete(id);
        if (isDeleted)
            return NoContent();
        else return NotFound();
    }
}