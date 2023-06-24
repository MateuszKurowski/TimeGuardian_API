using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

using TimeGuardian_API.Entities;
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

    #region Admin CRUD
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public ActionResult<IEnumerable<SessionDto>> GetAll()
        => Ok(_sessionService.GetAll());

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public ActionResult<SessionDto> Get([FromRoute] int id)
    {
        var session = _sessionService.GetById(id);
        return Ok(session);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public ActionResult<SessionDto> Update([FromBody] CreateSessionDto dto, [FromRoute] int id)
    {
        var session = _sessionService.Update(dto, id);
        return Ok(session);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}")]
    public ActionResult<SessionDto> Patch([FromBody] PatchSessionDto dto, [FromRoute] int id)
    {
        var session = _sessionService.Patch(dto, id);
        return Ok(session);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public ActionResult Create([FromBody] CreateSessionDto dto)
    {
        var id = _sessionService.Create(dto);
        return Created($"/api/session/{id}", null);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        _sessionService.Delete(id);
        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Route("start")]
    public ActionResult StartSession([FromBody] StartSessionDto dto)
    {
        var id = _sessionService.StartSession(dto);
        return Created($"/api/session/{id}", null);
    }

    [HttpPatch]
    [Route("end/{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult EndSession([FromBody] EndSessionDto dto, [FromRoute] int id)
    {
        var session = _sessionService.EndSession(dto, id);
        return Ok(session);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("user/{userId}")]
    public ActionResult<SessionDto> Get([FromRoute] int userId, [FromQuery] string? order, [FromQuery] string? orderBy, [FromQuery] int? typeId)
    {
        if (typeId.HasValue)
        {
            var sessions = _sessionService.GetSessionByUserIdAndTypeId(userId, (int)typeId, order, orderBy);
            return Ok(sessions);
        }
        else
        {
            var sessions = _sessionService.GetSessionByUserId(userId, order, orderBy);
            return Ok(sessions);
        }
    }
    #endregion

    #region Account

    [HttpGet]
    [Route("account/{id}")]
    public ActionResult<SessionDto> GetByAccount([FromRoute] int id)
    {
        var session = _sessionService.GetByIdByAccount(id, User);
        return Ok(session);
    }

    [HttpPatch]
    [Route("account/{id}")]
    public ActionResult<SessionDto> Patch([FromBody] PatchSessionDtoByAccount dto, [FromRoute] int id)
    {
        var session = _sessionService.PatchByAccount(dto, id, User);
        return Ok(session);
    }

    [HttpPost]
    [Route("account")]
    public ActionResult CreateByAccount([FromBody] CreateSessionDtoByAccount dto)
    {
        var id = _sessionService.CreateByAccount(dto, User);
        return Created($"/api/session/{id}", null);
    }

    [HttpDelete]
    [Route("account/{id}")]
    public ActionResult DeleteByAccount([FromRoute] int id)
    {
        _sessionService.DeleteByAccount(id, User);
        return NoContent();
    }

    [Route("account")]
    [HttpGet]
    public ActionResult<SessionDto> GetByAccount([FromQuery] string? order, [FromQuery] string? orderBy, [FromQuery] int? typeId)
    {
        if (typeId.HasValue)
        {
            var sessions = _sessionService.GetSessionByUserIdAndTypeIdByAccount(User, (int)typeId, order, orderBy);
            return Ok(sessions);
        }
        else
        {
            var sessions = _sessionService.GetSessionByUserIdByAccount(User, order, orderBy);
            return Ok(sessions);
        }
    }

    [HttpPost]
    [Route("account/start")]
    public ActionResult AccountStartSession([FromBody] int sessionTypeId)
    {
        var dto = new StartSessionDtoByAccount 
        { 
            SessionTypeId = sessionTypeId, 
        };
        var id = _sessionService.StartSessionByAccount(dto, User);
        return Created($"/api/session/{id}", null);
    }

    [HttpPatch]
    [Route("account/end/{id}")]
    public ActionResult AccountEndSession([FromRoute] int id)
    {
        var dto = new EndSessionDto();
        var session = _sessionService.EndSessionByAccount(dto, id, User);
        return Ok(session);
    }
    #endregion
}