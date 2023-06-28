using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using System.Data;
using System.Security.Claims;

using TimeGuardian_API.Authorization;
using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Exceptions;
using TimeGuardian_API.Models.SessionType;
using TimeGuardian_API.Models.User;

namespace TimeGuardian_API.Services;

public interface ISessionTypeService
{
    SessionTypeDto Create(CreateSessionTypeDtoByAccount dto, ClaimsPrincipal user);
    SessionTypeDto Create(CreateSessionTypeDto dto);
    void Delete(int id, ClaimsPrincipal user);
    IEnumerable<SessionTypeDto> GetAll();
    SessionTypeDto GetById(int id, ClaimsPrincipal user);
    SessionTypeDto GetByName(GetSessionTypeByNameAndUserIdDto dto);
    IEnumerable<SessionTypeDto> GetByUserId(ClaimsPrincipal user);
    IEnumerable<SessionTypeDto> GetByUserId(int userId);
    SessionTypeDto Update(CreateSessionTypeDto dto, int id, ClaimsPrincipal user);
}

public class SessionTypeService : ISessionTypeService
{
    private readonly ApiDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;

    public SessionTypeService(ApiDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _authorizationService = authorizationService;
    }

    public SessionTypeDto GetById(int id, ClaimsPrincipal user)
    {
        var sessionType = _dbContext
                                    .SessionTypes
                                    .Include(x => x.CreatedBy)
                                    .FirstOrDefault(st => st.Id == id) 
                                    ?? throw new NotFoundException(NotFoundException.Entities.SessionType);
        if (!sessionType.Default)
        {
            var authorizationResult = _authorizationService.AuthorizeAsync(user, sessionType, requirement: new SessionTypeSelfRequirment()).Result;
            if (!authorizationResult.Succeeded)
                throw new ForbidException();
        }
        
        return _mapper.Map<SessionTypeDto>(sessionType);
    }

    public SessionTypeDto GetByName(GetSessionTypeByNameAndUserIdDto dto)
    {
        var sessionType = _dbContext
                                    .SessionTypes
                                    .Include(x => x.CreatedBy)
                                    .FirstOrDefault(st => st.Name == dto.Name && st.CreatedById == dto.UserId);

        if (sessionType is null)
            throw new NotFoundException(NotFoundException.Entities.SessionType);
        else return _mapper.Map<SessionTypeDto>(sessionType);
    }

    public IEnumerable<SessionTypeDto> GetAll()
    {
        var sessionTypes = _dbContext.SessionTypes?.Include(x => x.CreatedBy)?.Select(sessionType => _mapper.Map<SessionTypeDto>(sessionType));

        return sessionTypes is null
            ? Enumerable.Empty<SessionTypeDto>()
            : sessionTypes;
    }

    public IEnumerable<SessionTypeDto> GetByUserId(ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        var sessionTypes = _dbContext.SessionTypes?.Where(sessionType => sessionType.CreatedById == userId)?.Include(x => x.CreatedBy)?.Select(sessionType => _mapper.Map<SessionTypeDto>(sessionType));

        return sessionTypes is null
            ? Enumerable.Empty<SessionTypeDto>()
            : sessionTypes;
    }

    public IEnumerable<SessionTypeDto> GetByUserId(int userId)
    {
        var sessionTypes = _dbContext.SessionTypes?.Where(sessionType => sessionType.CreatedById == userId)?.Include(x => x.CreatedBy)?.Select(sessionType => _mapper.Map<SessionTypeDto>(sessionType));

        return sessionTypes is null
            ? Enumerable.Empty<SessionTypeDto>()
            : sessionTypes;
    }

    public SessionTypeDto Update(CreateSessionTypeDto dto, int id, ClaimsPrincipal user)
    {
        var sessionType = _dbContext
                                     .SessionTypes
                                     .Include(x => x.CreatedBy)
                                     .FirstOrDefault(st => st.Id == id)
                                     ?? throw new NotFoundException(NotFoundException.Entities.SessionType);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, sessionType, requirement: new SessionTypeSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        sessionType.Name = dto.Name;
        _dbContext.SaveChanges();

        return _mapper.Map<SessionTypeDto>(sessionType);
    }

    public SessionTypeDto Create(CreateSessionTypeDtoByAccount dto, ClaimsPrincipal user)
    {
        var sessionType = _mapper.Map<SessionType>(dto);
        var userId = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        sessionType.CreatedById = int.Parse(userId);

        var userDAO = _dbContext.Users.FirstOrDefault(x => x.Id == sessionType.CreatedById)
            ?? throw new NotFoundException(NotFoundException.Entities.User);
        sessionType.CreatedBy = userDAO;

        _dbContext.SessionTypes.Add(sessionType);
        _dbContext.SaveChanges();

        return _mapper.Map<SessionTypeDto>(sessionType);
    }

    public SessionTypeDto Create(CreateSessionTypeDto dto)
    {
        var sessionType = _mapper.Map<SessionType>(dto);
        sessionType.CreatedById = dto.CreatedById;
        _dbContext.SessionTypes.Add(sessionType);

        var userDAO = _dbContext.Users.FirstOrDefault(x => x.Id == sessionType.CreatedById)
            ?? throw new NotFoundException(NotFoundException.Entities.User);
        sessionType.CreatedBy = userDAO;

        _dbContext.SaveChanges();

        return _mapper.Map<SessionTypeDto>(sessionType);
    }

    public void Delete(int id, ClaimsPrincipal user)
    {
        var sessionType = _dbContext
                        .SessionTypes
                        .FirstOrDefault(st => st.Id == id)
        ?? throw new NotFoundException(NotFoundException.Entities.SessionType);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, sessionType, requirement: new SessionTypeSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        _dbContext.SessionTypes.Remove(sessionType);
        _dbContext.SaveChanges();
    }
}