using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Exceptions;
using TimeGuardian_API.Models;
using TimeGuardian_API.Models.Login;

namespace TimeGuardian_API.Services;

public interface ILoginService
{
    RefreshTokenDto GenerateJwt(LoginDto dto);
    RefreshTokenDto RefreshJwt(RefreshTokenDto dto);
}

public class LoginService : ILoginService
{
    private readonly ApiDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUtilityService _utilityService;
    private readonly AuthenticationSettings _authenticationSettings;

    public LoginService(ApiDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IUtilityService utilityService)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _utilityService = utilityService;
        _authenticationSettings = authenticationSettings;
    }

    public RefreshTokenDto GenerateJwt(LoginDto dto)
    {
        var user = _dbContext.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == dto.Email && !u.Deleted)
            ?? throw new Exceptions.LoginException();

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            throw new LoginException();

        var token = GenerateNewTokenJwt(user);
        var refreshToken = GenerateRefreshToken(user);

        return new RefreshTokenDto
        {
            Token = token,
            RefreshToken = refreshToken,
        };
    }

    public RefreshTokenDto RefreshJwt(RefreshTokenDto dto)
    {
        var userId = _utilityService.GetUserIdFromToken(dto.Token);

        var user = _dbContext.Users.Include(x => x.Role).FirstOrDefault(x => x.Id == userId && !x.Deleted);
        if (user is null || user.RefreshToken != dto.RefreshToken)
            throw new BadRequestException();
        if (user.RefreshTokenExpiryTime <= DateTime.Now)
            throw new TokenExpireException();

        var token = GenerateNewTokenJwt(user);
        dto.Token = token;

        return dto;
    }

    private string GenerateRefreshToken(User user)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        _dbContext.SaveChanges();

        return refreshToken;
    }

    

    private string GenerateNewTokenJwt(User user)
    {
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
}