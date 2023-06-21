//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//using TimeGuardian_API.Data;
//using TimeGuardian_API.Entities;
//using TimeGuardian_API.Models.DTO;

//namespace TimeGuardian_API.Controllers;

//[Route("timeguardian/[controller]")]
//[ApiController]
//public class UsersController : ControllerBase
//{
//    private readonly ApiDbContext _context;

//    public UsersController(ApiDbContext context)
//    {
//        _context = context;
//    }

//    [Authorize]
//    // GET: api/Users
//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
//    {
//        return await _context.Users.ToListAsync();
//    }

//    [Authorize]
//    // GET: api/Users/5
//    [HttpGet("{id}")]
//    public async Task<ActionResult<User>> GetUser(int id)
//    {
//        var user = await _context.Users.FindAsync(id);

//        if (user == null)
//        {
//            return NotFound();
//        }

//        return user;
//    }

//    [Authorize]
//    // PUT: api/Users/5
//    [HttpPut("{id}")]
//    public async Task<IActionResult> PutUser(int id, UserDTO userDTO)
//    {
//        var user = await _context.Users.FindAsync(id);
//        if (user == null)
//        {
//            return NotFound();
//        }

//        user.Login = userDTO.Login;
//        user.Email = userDTO.Email;
//        user.Password = userDTO.Password;

//        await _context.SaveChangesAsync();

//        return NoContent();
//    }

//    // POST: api/Users
//    [HttpPost]
//    public async Task<ActionResult<User>> PostUser(UserDTO userDTO)
//    {
//        var existingUser = await _context.Users.FirstOrDefaultAsync(s => s.Login == userDTO.Login || s.Email == userDTO.Email);

//        if (existingUser != null)
//        {
//            return Conflict(new { message = $"User with name this login or email already exists." });
//        }

//        var user = new User
//        {
//            Email = userDTO.Email,
//            Password = userDTO.Password,
//            Login = userDTO.Login,
//            CreatedAt = DateTime.Now
//        };

//        _context.Users.Add(user);
//        await _context.SaveChangesAsync();

//        return CreatedAtAction("GetUser", new { id = user.Id }, user);
//    }

//    [Authorize]
//    // DELETE: api/Users/5
//    [HttpDelete("{id}")]
//    public async Task<ActionResult<User>> DeleteUser(int id)
//    {
//        var user = await _context.Users.FindAsync(id);
//        if (user == null)
//        {
//            return NotFound();
//        }

//        _context.Users.Remove(user);
//        await _context.SaveChangesAsync();

//        return user;
//    }

//    private bool UserExists(int id)
//    {
//        return _context.Users.Any(e => e.Id == id);
//    }
//}