using Microsoft.AspNetCore.Mvc;

using TimeGuardian_API.Models;
using TimeGuardian_API.Models.User;
using TimeGuardian_API.Services;

namespace TimeGuardian_API.Controllers;

[Route("api/user")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public ActionResult<UserDto> Get([FromRoute] int id)
    {
        var user = _userService.GetById(id);
        return user;
    }

    [HttpPut("{id}")]
    public ActionResult<UserDto> Update([FromBody] CreateUserDto dto, [FromRoute] int id)
    {
        var updatedUser = _userService.Update(dto, id);
        return Ok(updatedUser);
    }

    [HttpPost]
    public ActionResult<UserDto> Create([FromBody] CreateUserDto dto)
    {
        var id = _userService.Create(dto);
        return Created($"/api/user/{id}", null);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser([FromRoute] int id)
    {
        _userService.Delete(id);
        return NoContent();
    }
}