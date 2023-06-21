//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;

//using System.IdentityModel.Tokens.Jwt;
//using System.Text;

//using TimeGuardian_API.Data;
//using TimeGuardian_API.Models;

//namespace TimeGuardian_API.Controllers;

//[AllowAnonymous]
//[ApiController]
//[Route("timeguardian/[controller]")]
//public class LoginController : ControllerBase
//{
//    private readonly ApiDbContext _context;
//    private readonly IConfiguration _config;

//    public LoginController(IConfiguration config, ApiDbContext context)
//    {
//        _config = config;
//        _context = context;
//    }

//    /// <summary>
//    /// Login test
//    /// </summary>
//    /// <param name="login"> </param>
//    /// <returns> </returns>
//    [AllowAnonymous]
//    [HttpPost]
//    public async Task<IActionResult> Login([FromBody] LoginModel login)
//    {
//        IActionResult response = Unauthorized();

//        if (await AuthenticateUser(login))
//        {
//            var tokenString = GenerateJSONWebToken();
//            response = Ok(new { token = tokenString });
//        }

//        return response;
//    }

//    private string GenerateJSONWebToken()
//    {
//        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
//        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
//            _config["Jwt:Audience"],
//            null,
//            expires: DateTime.Now.AddMinutes(120),
//            signingCredentials: credentials);

//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }

//    //private async Task<bool> AuthenticateUser(LoginModel login)
//    //{
//    //    var users = await _context.Users.ToListAsync();
//    //    var lookingUser = users.Where(x => x.Login == login.Login).FirstOrDefault();
//    //    if (lookingUser is null) return false;
//    //    if (login.Login == lookingUser.Login && login.Password == lookingUser.Password)
//    //    {
//    //        return true;
//    //    }
//    //    return false;
//    //}
//}