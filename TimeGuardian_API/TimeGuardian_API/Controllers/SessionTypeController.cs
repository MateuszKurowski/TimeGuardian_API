using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TimeGuardian_API.Models.SessionType;
using TimeGuardian_API.Services;

namespace TimeGuardian_API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/sessiontype")]
public class SessionTypeController : ControllerBase
{
    private readonly ISessionTypeService _sessionTypeService;

    public SessionTypeController(ISessionTypeService sessionTypeService)
    {
        _sessionTypeService = sessionTypeService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<SessionTypeDto>> GetAll()
        => Ok(_sessionTypeService.GetAll());

    [HttpGet("{id}")]
    public ActionResult<SessionTypeDto> Get([FromRoute] int id)
    {
        var sessionType = _sessionTypeService.GetById(id);
        return Ok(sessionType);
    }

    [HttpPut("{id}")]
    public ActionResult<SessionTypeDto> Update([FromBody] CreateSessionTypeDto sessionTypeDto, [FromRoute] int id)
    {
        var sessionType = _sessionTypeService.Update(sessionTypeDto, id);
        return Ok(sessionType);
    }

    [HttpPost]
    public ActionResult Create([FromBody] CreateSessionTypeDto sessionTypeDto)
    {
        var id = _sessionTypeService.Create(sessionTypeDto);
        return Created($"/api/sessiontype/{id}", null);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        _sessionTypeService.Delete(id);
        return NoContent();
    }
}