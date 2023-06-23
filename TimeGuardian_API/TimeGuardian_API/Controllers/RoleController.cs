using Microsoft.AspNetCore.Mvc;

using TimeGuardian_API.Models.Role;
using TimeGuardian_API.Services;

namespace TimeGuardian_API.Controllers;

[Route("api/role")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
        => _roleService = roleService;

    [HttpGet]
    public ActionResult<IEnumerable<RoleDto>> GetAll()
        => Ok(_roleService.GetAll());

    [HttpGet("{id}")]
    public ActionResult<RoleDto> Get([FromRoute] int id)
    {
        var role = _roleService.GetById(id);
        return Ok(role);
    }

    [HttpPut("{id}")]
    public ActionResult<RoleDto> Put([FromBody] CreateRoleDto roleDto, [FromRoute] int id)
    {
        var role = _roleService.Update(roleDto, id);
        return Ok(role);
    }

    [HttpPost]
    public ActionResult Create([FromBody] CreateRoleDto roleDto)
    {
        var id = _roleService.Create(roleDto);
        return Created($"/api/role/{id}", null);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        _roleService.Delete(id);
        return NoContent();
    }
}