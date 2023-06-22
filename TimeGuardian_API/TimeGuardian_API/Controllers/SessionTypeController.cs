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
    public ActionResult<SessionTypeDto> Put(int id, SessionTypeDto sessionTypeDto)
    {
        var sessionType = _sessionTypeService.Update(id, sessionTypeDto);
        return Ok(sessionType);
    }

    [HttpPost]
    public ActionResult Create(SessionTypeDto sessionTypeDto)
    {
        var id = _sessionTypeService.Create(sessionTypeDto);
        return Created($"/api/sessiontype/{id}", null);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        _sessionTypeService.Delete(id);
        return NoContent();
    }
}