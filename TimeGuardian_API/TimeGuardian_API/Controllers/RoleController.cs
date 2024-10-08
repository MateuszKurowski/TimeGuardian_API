﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TimeGuardian_API.Models.Role;
using TimeGuardian_API.Services;

namespace TimeGuardian_API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/role")]
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
    public ActionResult<RoleDto> Create([FromBody] CreateRoleDto roleDto)
    {
        var role = _roleService.Create(roleDto);
        return Created($"/api/role/{role.Id}", role);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        _roleService.Delete(id);
        return NoContent();
    }
}