using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

using TimeGuardian_API.Models;
using TimeGuardian_API.Models.Login;
using TimeGuardian_API.Services;

namespace TimeGuardian_API.Controllers;

[Authorize]
[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult Login([FromBody] LoginDto dto)
    {
        var token = _loginService.GenerateJwt(dto);
        return Ok(token);
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("refresh")]
    public ActionResult RefreshToken([FromBody] OnlyRefreshTokenDto onlyRefreshToken)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

        var dto = new RefreshTokenDto()
        {
            Token = token,
            RefreshToken = onlyRefreshToken.RefreshToken
        };

        var newTokens = _loginService.RefreshJwt(dto);
        return Ok(newTokens);
    }
}