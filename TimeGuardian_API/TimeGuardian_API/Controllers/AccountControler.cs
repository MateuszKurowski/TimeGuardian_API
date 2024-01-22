using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

using TimeGuardian_API.Models;
using TimeGuardian_API.Models.User;
using TimeGuardian_API.Services;

namespace TimeGuardian_API.Controllers;

[ApiController]
[Route("api/account")]
public class AccountControler : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUtilityService _utilityService;

    public AccountControler(IUserService userService, IUtilityService utilityService)
    {
        _userService = userService;
        _utilityService = utilityService;
    }

    [HttpGet]
    public ActionResult<UserDto> Get()
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var id = _utilityService.GetUserIdFromToken(token);
        var user = _userService.GetById(id);
        return user;
    }

    [HttpPut]
    public ActionResult<UserDto> Update([FromBody] CreateUserDto dto)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var id = _utilityService.GetUserIdFromToken(token);
        var updatedUser = _userService.Update(dto, id);
        return Ok(updatedUser);
    }

    [HttpDelete]
    public ActionResult DeleteUser()
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var id = _utilityService.GetUserIdFromToken(token);
        _userService.Delete(id);
        return NoContent();
    }

    [HttpPatch]
    public ActionResult<UserDto> Patch([FromBody] PatchUserDto dto)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var id = _utilityService.GetUserIdFromToken(token);
        var user = _userService.Patch(dto, id);
        return Ok(user);
    }

    [HttpPost]
    [Route("change_password")]
    public ActionResult ChangePassword([FromBody] PasswordDto dto)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var id = _utilityService.GetUserIdFromToken(token);
        _userService.ChangePassword(dto, id);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("forgot")]
    public ActionResult Forgot([FromBody] ForgotPassword forgotPassword)
    {
        _userService.ForgotPassword(forgotPassword);
        return NoContent();
    }
}