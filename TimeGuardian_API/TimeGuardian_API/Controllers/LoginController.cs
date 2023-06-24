using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using TimeGuardian_API.Data;
using TimeGuardian_API.Models;
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
    public ActionResult RefreshToken([FromBody] string refreshToken)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

        var dto = new RefreshTokenDto()
        {
            Token = token,
            RefreshToken = refreshToken
        };

        var newTokens = _loginService.RefreshJwt(dto);
        return Ok(newTokens);
    }
}