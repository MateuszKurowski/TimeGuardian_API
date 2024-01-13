using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

using TimeGuardian_API.Authorization;
using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Exceptions;
using TimeGuardian_API.Models.Pomodoro;
using TimeGuardian_API.Models.SessionType;

namespace TimeGuardian_API.Services;

public interface IPomodoroService
{
    PomodoroDto Create(CreateByAdminPomodoroDto dto);
    PomodoroDto Create(CreatePomodoroDto dto, ClaimsPrincipal user);
    void Delete(int id, ClaimsPrincipal user);
    IEnumerable<PomodoroDto> GetAll();
    PomodoroDto GetById(int id, ClaimsPrincipal user);
    IEnumerable<PomodoroDto> GetByUserId(ClaimsPrincipal user);
    IEnumerable<PomodoroDto> GetByUserId(int userId);
    PomodoroDto Patch(CreatePomodoroDto dto, int id, ClaimsPrincipal user);
    PomodoroDto Update(CreatePomodoroDto dto, int id, ClaimsPrincipal user);
}

public class PomodoroService : IPomodoroService
{
    private readonly ApiDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;

    public PomodoroService(ApiDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _authorizationService = authorizationService;
    }

    public PomodoroDto GetById(int id, ClaimsPrincipal user)
    {
        var pomodoro = _dbContext
                                    .Pomodoro
                                    .FirstOrDefault(p => p.Id == id)
                                    ?? throw new NotFoundException(NotFoundException.Entities.Pomodoro);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, pomodoro, requirement: new PomodoroSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        return _mapper.Map<PomodoroDto>(pomodoro);
    }

    public IEnumerable<PomodoroDto> GetByUserId(int userId)
    {
        var pomodoros = _dbContext.Pomodoro?.Where(pomodoro => pomodoro.UserId == userId)?.Select(pomodoro => _mapper.Map<PomodoroDto>(pomodoro));

        return pomodoros is null
            ? Enumerable.Empty<PomodoroDto>()
            : pomodoros;
    }

    public IEnumerable<PomodoroDto> GetByUserId(ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        var pomodoros = _dbContext.Pomodoro?.Where(pomodoro => pomodoro.UserId == userId)?.Select(pomodoro => _mapper.Map<PomodoroDto>(pomodoro));

        return pomodoros is null
            ? Enumerable.Empty<PomodoroDto>()
            : pomodoros;
    }

    public IEnumerable<PomodoroDto> GetAll()
    {
        var pomodoros = _dbContext.Pomodoro?.Select(pomodoro => _mapper.Map<PomodoroDto>(pomodoro));

        return pomodoros is null
            ? Enumerable.Empty<PomodoroDto>()
            : pomodoros;
    }

    public PomodoroDto Update(CreatePomodoroDto dto, int id, ClaimsPrincipal user)
    {
        if (dto.DurationInMinutes < 1)
            throw new BadRequestException("Duration canno be less than 0 minutes.");

        var pomodoro = _dbContext
                                     .Pomodoro
                                     .FirstOrDefault(p => p.Id == id)
                                     ?? throw new NotFoundException(NotFoundException.Entities.Pomodoro);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, pomodoro, requirement: new PomodoroSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        if (dto.DurationInMinutes is null || dto.Date is null)
            throw new BadRequestException();

        pomodoro.Duration = (int)dto.DurationInMinutes;
        pomodoro.Date = (DateTime)dto.Date;
        _dbContext.SaveChanges();

        return _mapper.Map<PomodoroDto>(pomodoro);
    }

    public PomodoroDto Patch(CreatePomodoroDto dto, int id, ClaimsPrincipal user)
    {
        if (dto.DurationInMinutes < 1)
            throw new BadRequestException("Duration canno be less than 0 minutes.");

        var pomodoro = _dbContext
                                     .Pomodoro
                                     .FirstOrDefault(p => p.Id == id)
                                     ?? throw new NotFoundException(NotFoundException.Entities.Pomodoro);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, pomodoro, requirement: new PomodoroSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        var wasChanges = false;
        if (dto.DurationInMinutes != null)
        {
            pomodoro.Duration = (int)dto.DurationInMinutes;
            wasChanges = true;
        }
        if (dto.Date != null)
        {
            pomodoro.Date = (DateTime)dto.Date;
            wasChanges = true;
        }
        if (wasChanges)
            _dbContext.SaveChanges();

        return _mapper.Map<PomodoroDto>(pomodoro);
    }

    public PomodoroDto Create(CreatePomodoroDto dto, ClaimsPrincipal user)
    {
        if (dto.DurationInMinutes < 1)
            throw new BadRequestException("Duration canno be less than 0 minutes.");

        var pomodoro = _mapper.Map<Pomodoro>(dto);
        var userId = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        pomodoro.UserId = int.Parse(userId);

        var userDAO = _dbContext.Users.FirstOrDefault(x => x.Id == pomodoro.UserId)
            ?? throw new NotFoundException(NotFoundException.Entities.User);
        pomodoro.User = userDAO;

        _dbContext.Pomodoro.Add(pomodoro);
        _dbContext.SaveChanges();

        return _mapper.Map<PomodoroDto>(pomodoro);
    }

    public PomodoroDto Create(CreateByAdminPomodoroDto dto)
    {
        if (dto.DurationInMinutes < 1)
            throw new BadRequestException("Duration canno be less than 0 minutes.");

        var pomodoro = _mapper.Map<Pomodoro>(dto);
        pomodoro.UserId = dto.UserId;
        _dbContext.Pomodoro.Add(pomodoro);

        var userDAO = _dbContext.Users.FirstOrDefault(x => x.Id == pomodoro.UserId)
            ?? throw new NotFoundException(NotFoundException.Entities.User);
        pomodoro.User = userDAO;

        _dbContext.SaveChanges();

        return _mapper.Map<PomodoroDto>(pomodoro);
    }

    public void Delete(int id, ClaimsPrincipal user)
    {
        var pomodoro = _dbContext
                        .Pomodoro
                        .FirstOrDefault(p => p.Id == id)
        ?? throw new NotFoundException(NotFoundException.Entities.Pomodoro);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, pomodoro, requirement: new PomodoroSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        _dbContext.Pomodoro.Remove(pomodoro);
        _dbContext.SaveChanges();
    }
}
