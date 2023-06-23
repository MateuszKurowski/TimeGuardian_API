using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Exceptions;
using TimeGuardian_API.Models;

namespace TimeGuardian_API.Services;

public interface ILoginService
{
    string GenerateJwt(LoginDto dto);
}

public class LoginService : ILoginService
{
    private readonly ApiDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;

    public LoginService(ApiDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _authenticationSettings = authenticationSettings;
    }

    public string GenerateJwt(LoginDto dto)
    {
        var user = _dbContext.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == dto.Email)
            ?? throw new LoginException();

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            throw new LoginException();

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.Name),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddHours(_authenticationSettings.JwtExpireHours);

        var token = new JwtSecurityToken(
            issuer: _authenticationSettings.JwtIssuer,
            audience: _authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: cred);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    //public string RefreshJwt()
    //{

    //}
}
