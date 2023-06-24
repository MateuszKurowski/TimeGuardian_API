using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using TimeGuardian_API.Exceptions;

namespace TimeGuardian_API.Services;

public interface IUtilityService
{
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    int GetUserIdFromToken(string token);
}

public class UtilityService : IUtilityService
{
    private readonly AuthenticationSettings _authenticationSettings;

    public UtilityService(AuthenticationSettings authenticationSettings)
    {
        _authenticationSettings = authenticationSettings;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey)),
        };

        var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new LoginException("Invalid token");

        return principal;
    }

    public int GetUserIdFromToken(string token)
    {
        var identity = GetPrincipalFromExpiredToken(token);
        if (identity != null)
        {
            var id = identity.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(id, out var userId))
                return userId;
        }
        throw new Exception("Claim dosen't have user ID.");
    }
}
