using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TimeGuardian_API.Models;
using TimeGuardian_API.Models.User;
using TimeGuardian_API.Services;

namespace TimeGuardian_API.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [HttpPatch]
    [Route("{id}/role/{roleId}")]
    public ActionResult ChangeRole([FromRoute] int id, [FromRoute] int roleId)
    {
        _userService.ChangeRole(id, roleId);
        return Ok();
    }

    [Authorize(Policy = "SelfRequirment")]
    [HttpGet("{id}")]
    public ActionResult<UserDto> Get([FromRoute] int id)
    {
        var user = _userService.GetById(id);
        return user;
    }

    [Authorize(Policy = "SelfRequirment")]
    [HttpPut("{id}")]
    public ActionResult<UserDto> Update([FromBody] CreateUserDto dto, [FromRoute] int id)
    {
        var updatedUser = _userService.Update(dto, id);
        return Ok(updatedUser);
    }

    [Authorize(Policy = "SelfRequirment")]
    [HttpPatch("{id}")]
    public ActionResult<UserDto> Patch([FromBody] PatchUserDto dto, [FromRoute] int id)
    {
        var user = _userService.Patch(dto, id);
        return Ok(user);
    }

    [AllowAnonymous]
    [HttpPost]
    public ActionResult<UserDto> Create([FromBody] CreateUserDto dto)
    {
        var user = _userService.Create(dto);
        return Created($"/api/user/{user.Id}", user);
    }

    [Authorize(Policy = "SelfRequirment")]
    [HttpDelete("{id}")]
    public ActionResult DeleteUser([FromRoute] int id)
    {
        _userService.Delete(id);
        return NoContent();
    }


}