//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//using TimeGuardian_API.Data;
//using TimeGuardian_API.Entities;

//namespace TimeGuardian_API.Controllers;

////[Authorize]
//[Route("timeguardian/[controller]")]
//[ApiController]
//public class SessionsController : ControllerBase
//{
//    private readonly ApiDbContext _context;

//    public SessionsController(ApiDbContext context)
//    {
//        _context = context;
//    }

//    // GET: api/Sessions
//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<Session>>> GetSessions()
//    {
//        return await _context.Sessions.Include(x => x.User).Include(x => x.SessionType).ToListAsync();
//    }

//    //// GET: api/Sessions
//    //[Route("GetSessionsByUserId/{userId}")]
//    //[HttpGet]
//    //public async Task<ActionResult<IEnumerable<Session>>> GetSessionsByUserId(int userId)
//    //{
//    //    var sessions = await _context.Sessions.Include(x => x.User).Include(x => x.SessionType).Where(x => x.UserId == userId).ToListAsync();
//    //    if (sessions.Any())
//    //        return Ok(sessions);
//    //    else return NotFound();
//    //}

//    //// GET: api/Sessions/5
//    //[HttpGet("{id}")]
//    //public async Task<ActionResult<Session>> GetSession(int id)
//    //{
//    //    var session = await _context.Sessions.Include(x => x.User).Include(x => x.SessionType).Where(x => x.Id == id).FirstAsync();

//    //    if (session == null)
//    //        return NotFound();
//    //    else
//    //        return session;
//    //}

//    //// PUT: api/Sessions/5
//    //[HttpPut("{id}")]
//    //public async Task<IActionResult> PutSession(int id, SessionUpdateDTO sessionDTO)
//    //{
//    //    var session = await _context.Sessions.FindAsync(id);
//    //    if (session == null)
//    //    {
//    //        return NotFound();
//    //    }

//    //    session.StartTime = sessionDTO.StartTime;
//    //    session.EndTime = sessionDTO.EndTime;
//    //    session.Duration = sessionDTO.Duration;
//    //    session.SessionTypeId = sessionDTO.SessionTypeId;

//    //    await _context.SaveChangesAsync();

//    //    return NoContent();
//    //}

//    //// POST: api/Sessions
//    //[HttpPost]
//    //public async Task<ActionResult<Session>> PostSession(SessionDTO sessionDTO)
//    //{
//    //    var session = new Session
//    //    {
//    //        UserId = sessionDTO.UserId,
//    //        EndTime = sessionDTO.EndTime,
//    //        StartTime = sessionDTO.StartTime,
//    //        Duration = sessionDTO.Duration,
//    //    };

//    //    _context.Sessions.Add(session);
//    //    await _context.SaveChangesAsync();

//    //    return CreatedAtAction("GetSession", new { id = session.Id }, session);
//    //}

//    //// DELETE: api/Sessions/5
//    //[HttpDelete("{id}")]
//    //public async Task<ActionResult<Session>> DeleteSession(int id)
//    //{
//    //    var session = await _context.Sessions.FindAsync(id);
//    //    if (session == null)
//    //    {
//    //        return NotFound();
//    //    }

//    //    _context.Sessions.Remove(session);
//    //    await _context.SaveChangesAsync();

//    //    return session;
//    //}

//    //private bool SessionExists(int id)
//    //{
//    //    return _context.Sessions.Any(e => e.Id == id);
//    //}
//}