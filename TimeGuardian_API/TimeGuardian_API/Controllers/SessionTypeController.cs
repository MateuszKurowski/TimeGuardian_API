using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TimeGuardian_API.Models.SessionType;
using TimeGuardian_API.Services;

namespace TimeGuardian_API.Controllers;

[Authorize]
[ApiController]
[Route("api/sessiontype")]
public class SessionTypeController : ControllerBase
{
    private readonly ISessionTypeService _sessionTypeService;

    public SessionTypeController(ISessionTypeService sessionTypeService)
    {
        _sessionTypeService = sessionTypeService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public ActionResult<IEnumerable<SessionTypeDto>> GetAll()
        => Ok(_sessionTypeService.GetAll());

    [HttpGet("{id}")]
    public ActionResult<SessionTypeDto> Get([FromRoute] int id)
    {
        var sessionType = _sessionTypeService.GetById(id, User);
        return Ok(sessionType);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [Route("name")]
    public ActionResult<SessionTypeDto> GetByNameAndUserId([FromBody] GetSessionTypeByNameAndUserIdDto dto)
    {
        var sessionType = _sessionTypeService.GetByName(dto);
        return Ok(sessionType);
    }

    [Authorize(Roles = "Admin")]
    [Route("user/{userId}")]
    [HttpGet]
    public ActionResult<IEnumerable<SessionTypeDto>> GetByUserId([FromRoute] int userId)
    {
        var sessionTypes = _sessionTypeService.GetByUserId(userId);
        return Ok(sessionTypes);
    }

    [Route("account")]
    [HttpGet]
    public ActionResult<IEnumerable<SessionTypeDto>> GetByAccount()
    {
        var sessionTypes = _sessionTypeService.GetByUserId(User);
        return Ok(sessionTypes);
    }

    [HttpPut("{id}")]
    public ActionResult<SessionTypeDto> Update([FromBody] CreateSessionTypeDto dto, [FromRoute] int id)
    {
        var sessionType = _sessionTypeService.Update(dto, id, User);
        return Ok(sessionType);
    }

    [HttpPost]
    public ActionResult Create([FromBody] CreateSessionTypeDto sessionTypeDto)
    {
        var sessionType = _sessionTypeService.Create(sessionTypeDto);
        return Created($"/api/sessiontype/{sessionType.Id}", sessionType);
    }

    [HttpPost]
    [Route("account")]
    public ActionResult Create([FromBody] CreateSessionTypeDtoByAccount dto)
    {
        var sessionType = _sessionTypeService.Create(dto, User);
        return Created($"/api/sessiontype/{sessionType.Id}", sessionType);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        _sessionTypeService.Delete(id, User);
        return NoContent();
    }
}