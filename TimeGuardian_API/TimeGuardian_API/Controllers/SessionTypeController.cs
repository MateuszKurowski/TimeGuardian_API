using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TimeGuardian_API.Data;
using TimeGuardian_API.Models;
using TimeGuardian_API.Models.DTO;

namespace TimeGuardian_API.Controllers;

[Authorize]
[Route("timeguardian/[controller]")]
[ApiController]
public class SessionTypesController : ControllerBase
{
    private readonly ApiContext _context;

    public SessionTypesController(ApiContext context)
    {
        _context = context;
    }

    // GET: api/SessionTypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SessionType>>> GetSessionTypes()
    {
        return await _context.SessionTypes.ToListAsync();
    }

    // GET: api/SessionTypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SessionType>> GetSessionType(int id)
    {
        var sessionType = await _context.SessionTypes.FindAsync(id);

        if (sessionType == null)
        {
            return NotFound();
        }

        return sessionType;
    }

    // PUT: api/SessionTypes/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSessionType(int id, SessionTypeDTO sessionTypeDTO)
    {
        var sessionType = await _context.SessionTypes.FindAsync(id);
        if (sessionType == null)
        {
            return NotFound();
        }

        var existingSessionType = await _context.SessionTypes.FirstOrDefaultAsync(s => s.Name == sessionTypeDTO.Name && s.Id != id);

        if (existingSessionType != null)
        {
            return Conflict(new { message = $"SessionType with name {sessionTypeDTO.Name} already exists." });
        }

        sessionType.Name = sessionTypeDTO.Name;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SessionTypeExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/SessionTypes
    [HttpPost]
    public async Task<ActionResult<SessionType>> PostSessionType(SessionTypeDTO sessionTypeDTO)
    {
        var existingSessionType = await _context.SessionTypes.FirstOrDefaultAsync(s => s.Name == sessionTypeDTO.Name);

        if (existingSessionType != null)
        {
            return Conflict(new { message = $"SessionType with name {sessionTypeDTO.Name} already exists." });
        }

        var sessionType = new SessionType
        {
            Name = sessionTypeDTO.Name,
        };

        _context.SessionTypes.Add(sessionType);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetSessionType", new { id = sessionType.Id }, sessionType);
    }

    // DELETE: api/SessionTypes/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<SessionType>> DeleteSessionType(int id)
    {
        var sessionType = await _context.SessionTypes.FindAsync(id);
        if (sessionType == null)
        {
            return NotFound();
        }

        _context.SessionTypes.Remove(sessionType);
        await _context.SaveChangesAsync();

        return sessionType;
    }

    private bool SessionTypeExists(int id)
    {
        return _context.SessionTypes.Any(e => e.Id == id);
    }
}