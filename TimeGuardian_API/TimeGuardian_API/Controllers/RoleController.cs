using Microsoft.AspNetCore.Mvc;

using TimeGuardian_API.Entities;
using TimeGuardian_API.Models;
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
    public ActionResult<IEnumerable<Role>> GetAll()
        => Ok(_roleService.GetAll());

    [HttpGet("{id}")]
    public ActionResult<Role> Get([FromRoute] int id)
    {
        var role = _roleService.GetById(id);

        if (role == null)
            return NotFound();

        return Ok(role);
    }

    [HttpPut("{id}")]
    public ActionResult<Role> Put(int id, RoleDto roleDto)
    {
        var existingrole = _roleService.GetByName(roleDto.Name);
        if (existingrole != null)
            return Conflict(new { message = $"Role with name {roleDto.Name} already exists." });

        var role = _roleService.Update(id, roleDto);
        if (role is null)
            return NotFound();

        return Ok(role);
    }

    [HttpPost]
    public ActionResult Create(RoleDto roleDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingrole = _roleService.GetByName(roleDto.Name);
        if (existingrole != null)
            return Conflict(new { message = $"role with name {roleDto.Name} already exists." });

        var id = _roleService.Create(roleDto);

        return Created($"/api/role/{id}", null);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var isDeleted = _roleService.Delete(id);
        if (isDeleted)
            return NoContent();
        else return NotFound();
    }
}