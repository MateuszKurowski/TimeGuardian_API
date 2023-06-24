using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

using TimeGuardian_API.Authorization;
using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Exceptions;
using TimeGuardian_API.Models.Session;

namespace TimeGuardian_API.Services;

public interface ISessionService
{
    int Create(CreateSessionDto dto);
    int CreateByAccount(CreateSessionDtoByAccount dto, ClaimsPrincipal user);
    void Delete(int id);
    void DeleteByAccount(int id, ClaimsPrincipal user);
    SessionDto EndSession(EndSessionDto dto, int id);
    SessionDto EndSessionByAccount(EndSessionDto dto, int id, ClaimsPrincipal user);
    IEnumerable<SessionDto> GetAll();
    IEnumerable<SessionDto> GetAllByAccount(ClaimsPrincipal user);
    SessionDto GetById(int id);
    SessionDto GetByIdByAccount(int id, ClaimsPrincipal user);
    IEnumerable<SessionDto> GetSessionByUserId(int userId, string? order = null, string? orderBy = null);
    IEnumerable<SessionDto> GetSessionByUserIdAndTypeId(int userId, int typeId, string? order, string? orderBy);
    IEnumerable<SessionDto> GetSessionByUserIdAndTypeIdByAccount(ClaimsPrincipal user, int typeId, string? order, string? orderBy);
    IEnumerable<SessionDto> GetSessionByUserIdByAccount(ClaimsPrincipal user, string? order = null, string? orderBy = null);
    SessionDto Patch(PatchSessionDto dto, int id);
    SessionDto PatchByAccount(PatchSessionDtoByAccount dto, int id, ClaimsPrincipal user);
    int StartSession(StartSessionDto dto);
    int StartSessionByAccount(StartSessionDtoByAccount dto, ClaimsPrincipal user);
    SessionDto Update(CreateSessionDto dto, int id);
}

public class SessionService : ISessionService
{
    private readonly ApiDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IAuthorizationService _authorizationService;

    public enum Order
    {
        Asc,
        Desc
    }

    public enum OrderBy
    {
        StartTime,
        EndTime,
        Duration,
        SessiontTypeName,
        UserId,
    }

    public SessionService(ApiDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _authorizationService = authorizationService;
    }

    #region CRUD
    public SessionDto GetById(int id)
    {
        var session = _dbContext
                        .Sessions
                        .Include(x => x.User)
                        .Include(x => x.SessionType)
                        .FirstOrDefault(r => r.Id == id && !r.Deleted)
                        ?? throw new NotFoundException(NotFoundException.Entities.Session);

        return _mapper.Map<SessionDto>(session);
    }

    public IEnumerable<SessionDto> GetAll()
    {
        var sessions = _dbContext.Sessions
            ?.Include(x => x.User)
            ?.Include(x => x.SessionType)
            ?.Where(x => !x.Deleted)
            ?.Select(session => _mapper.Map<SessionDto>(session));

        return sessions is null
            ? Enumerable.Empty<SessionDto>()
            : sessions;
    }

    public SessionDto Update(CreateSessionDto dto, int id)
    {
        var session = _dbContext
                        .Sessions
                        ?.Include(x => x.User)
                        ?.Include(x => x.SessionType)
                        .FirstOrDefault(r => r.Id == id && !r.Deleted)
                        ?? throw new NotFoundException(NotFoundException.Entities.Session);

        session.StartTime = dto.StartTime;
        session.EndTime = dto.EndTime;
        if (session.EndTime.HasValue)
            session.Duration = ((int)(session.EndTime.Value - session.StartTime).TotalSeconds);

        var userDao = _dbContext.Users.FirstOrDefault(x => x.Id == dto.UserId && !x.Deleted);
        if (userDao != null)
            session.UserId = dto.UserId;
        var sessionType = _dbContext.SessionTypes.FirstOrDefault(x => x.Id == dto.SessionTypeId);
        if (sessionType != null)
            session.SessionTypeId = dto.SessionTypeId;

        _dbContext.SaveChanges();

        return _mapper.Map<SessionDto>(session);
    }

    public SessionDto Patch(PatchSessionDto dto, int id)
    {
        var session = _dbContext
                        .Sessions
                        .Include(x => x.User)
                        .Include(x => x.SessionType)
                        .FirstOrDefault(r => r.Id == id && !r.Deleted)
                        ?? throw new NotFoundException(NotFoundException.Entities.Session);

        if (dto.StartTime.HasValue)
            session.StartTime = dto.StartTime.Value;

        if (session.EndTime.HasValue)
            session.EndTime = dto.EndTime;

        if (session.EndTime.HasValue)
            session.Duration = ((int)(session.EndTime.Value - session.StartTime).TotalSeconds);

        if (dto.UserId.HasValue)
        {
            var userDao = _dbContext.Users.FirstOrDefault(x => x.Id == dto.UserId && !x.Deleted);
            if (userDao != null)
            {
                session.UserId = dto.UserId.Value;
                session.User = userDao;
            }
        }

        if (dto.SessionTypeId.HasValue)
        {
            var sessionType = _dbContext.SessionTypes.FirstOrDefault(x => x.Id == dto.SessionTypeId);
            if (sessionType != null)
            {
                session.SessionTypeId = dto.SessionTypeId.Value;
                session.SessionType = sessionType;
            }
        }

        _dbContext.SaveChanges();

        return _mapper.Map<SessionDto>(session);
    }

    public int Create(CreateSessionDto dto)
    {
        var session = _mapper.Map<Session>(dto);

        if (session.EndTime.HasValue)
            session.Duration = ((int)(session.EndTime.Value - session.StartTime).TotalSeconds);

        _dbContext.Sessions.Add(session);
        _dbContext.SaveChanges();

        return session.Id;
    }

    public void Delete(int id)
    {
        var session = _dbContext
                        .Sessions
                        .FirstOrDefault(r => r.Id == id && !r.Deleted)
                        ?? throw new NotFoundException(NotFoundException.Entities.Session);

        session.Deleted = true;
        _dbContext.SaveChanges();
    }
    #endregion

    public IEnumerable<SessionDto> GetSessionByUserId(int userId, string? order = null, string? orderBy = null)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId && !x.Deleted)
            ?? throw new NotFoundException(NotFoundException.Entities.User);

        var sessions = _dbContext.Sessions.Where(s => s.UserId == userId && !s.Deleted).Include(x => x.User).Include(x => x.SessionType);
        if (sessions is null || !sessions.Any())
            return Enumerable.Empty<SessionDto>();
        var orderEnum = CheckOrder(order);
        var orderByEnum = CheckOrderBy(orderBy);

        return OrderSession(sessions, orderEnum, orderByEnum);
    }

    public IEnumerable<SessionDto> GetSessionByUserIdAndTypeId(int userId, int typeId, string? order, string? orderBy)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId && !x.Deleted)
            ?? throw new NotFoundException(NotFoundException.Entities.User);

        var sessions = _dbContext.Sessions.Where(s => s.UserId == userId && !s.Deleted).Include(x => x.User).Include(x => x.SessionType).Where(x => x.SessionTypeId == typeId);
        if (sessions is null || !sessions.Any())
            return Enumerable.Empty<SessionDto>();
        var orderEnum = CheckOrder(order);
        var orderByEnum = CheckOrderBy(orderBy);

        return OrderSession(sessions, orderEnum, orderByEnum);
    }

    public int StartSession(StartSessionDto dto)
    {
        dto.StartTime ??= DateTime.Now;

        var session = new Session()
        {
            UserId = dto.UserId,
            StartTime = (DateTime)dto.StartTime,
            SessionTypeId = dto.SessionTypeId,
            Deleted = false
        };
        _dbContext.Sessions.Add(session);
        _dbContext.SaveChanges();
        return session.Id;
    }

    public SessionDto EndSession(EndSessionDto dto, int id)
    {
        dto.EndTime ??= DateTime.Now;

        var session = _dbContext.Sessions.Include(x => x.SessionType).Include(x => x.User).FirstOrDefault(x => x.Id == id && !x.Deleted)
            ?? throw new NotFoundException(NotFoundException.Entities.Session);

        if (session.EndTime.HasValue)
            throw new SessionAlreadyEndException();

        if (dto.EndTime.Value <= session.StartTime)
            throw new BadRequestException("End time cannot be earlier than start time.");

        session.EndTime = dto.EndTime;
        session.Duration = ((int)(session.EndTime.Value - session.StartTime).TotalSeconds);

        _dbContext.SaveChanges();

        return _mapper.Map<SessionDto>(session);
    }


    #region ByAccount

    public int StartSessionByAccount(StartSessionDtoByAccount dto, ClaimsPrincipal user)
    {
        var userId = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("userId is null");

        dto.StartTime ??= DateTime.Now;

        var session = new Session()
        {
            UserId = int.Parse(userId),
            StartTime = (DateTime)dto.StartTime,
            SessionTypeId = dto.SessionTypeId,
            Deleted = false
        };
        _dbContext.Sessions.Add(session);
        _dbContext.SaveChanges();
        return session.Id;
    }

    public SessionDto EndSessionByAccount(EndSessionDto dto, int id, ClaimsPrincipal user)
    {
        dto.EndTime ??= DateTime.Now;

        var session = _dbContext.Sessions.Include(x => x.SessionType).Include(x => x.User).FirstOrDefault(x => x.Id == id && !x.Deleted)
            ?? throw new NotFoundException(NotFoundException.Entities.Session);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, session, new ResourceSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        if (session.EndTime.HasValue)
            throw new SessionAlreadyEndException();
        
        if (dto.EndTime.Value <= session.StartTime)
            throw new BadRequestException("End time cannot be earlier than start time.");

        session.EndTime = dto.EndTime;
        session.Duration = ((int)(session.EndTime.Value - session.StartTime).TotalSeconds);

        _dbContext.SaveChanges();

        return _mapper.Map<SessionDto>(session);
    }

    public SessionDto GetByIdByAccount(int id, ClaimsPrincipal user)
    {
        var session = _dbContext
                        .Sessions
                        .Include(x => x.User)
                        .Include(x => x.SessionType)
                        .FirstOrDefault(r => r.Id == id && !r.Deleted)
                        ?? throw new NotFoundException(NotFoundException.Entities.Session);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, session, new ResourceSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        return _mapper.Map<SessionDto>(session);
    }

    public IEnumerable<SessionDto> GetAllByAccount(ClaimsPrincipal user)
    {
        var userId = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("userId is null");

        var sessions = _dbContext.Sessions
            ?.Include(x => x.User)
            ?.Include(x => x.SessionType)
            ?.Where(x => !x.Deleted && x.UserId == int.Parse(userId))
            ?.Select(session => _mapper.Map<SessionDto>(session));

        return sessions is null
            ? Enumerable.Empty<SessionDto>()
            : sessions;
    }

    public SessionDto PatchByAccount(PatchSessionDtoByAccount dto, int id, ClaimsPrincipal user)
    {
        var session = _dbContext
                        .Sessions
                        .Include(x => x.User)
                        .Include(x => x.SessionType)
                        .FirstOrDefault(r => r.Id == id && !r.Deleted)
                        ?? throw new NotFoundException(NotFoundException.Entities.Session);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, session, new ResourceSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        if (dto.StartTime.HasValue)
        {
            if (session.EndTime.HasValue && session.EndTime.Value <= session.StartTime)
                throw new BadRequestException("End time cannot be earlier than start time.");
            session.StartTime = dto.StartTime.Value;
        }

        if (session.EndTime.HasValue)
        {
            session.EndTime = dto.EndTime;
            if (session.EndTime.Value <= session.StartTime)
                throw new BadRequestException("End time cannot be earlier than start time.");
        }

        if (session.EndTime.HasValue)
            session.Duration = ((int)(session.EndTime.Value - session.StartTime).TotalSeconds);

        if (dto.SessionTypeId.HasValue)
        {
            var sessionType = _dbContext.SessionTypes.FirstOrDefault(x => x.Id == dto.SessionTypeId);
            if (sessionType != null)
            {
                session.SessionTypeId = dto.SessionTypeId.Value;
                session.SessionType = sessionType;
            }
        }

        _dbContext.SaveChanges();

        return _mapper.Map<SessionDto>(session);
    }

    public int CreateByAccount(CreateSessionDtoByAccount dto, ClaimsPrincipal user)
    {
        var session = _mapper.Map<Session>(dto);

        if (session.EndTime.HasValue)
            session.Duration = ((int)(session.EndTime.Value - session.StartTime).TotalSeconds);
        var userId = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("userId is null");
        session.UserId = int.Parse(userId);

        if (session.EndTime.HasValue && session.EndTime.Value <= session.StartTime)
            throw new BadRequestException("End time cannot be earlier than start time.");

        _dbContext.Sessions.Add(session);
        _dbContext.SaveChanges();

        return session.Id;
    }

    public void DeleteByAccount(int id, ClaimsPrincipal user)
    {
        var session = _dbContext
                        .Sessions
                        .FirstOrDefault(r => r.Id == id && !r.Deleted)
                        ?? throw new NotFoundException(NotFoundException.Entities.Session);

        var authorizationResult = _authorizationService.AuthorizeAsync(user, session, new ResourceSelfRequirment()).Result;
        if (!authorizationResult.Succeeded)
            throw new ForbidException();

        session.Deleted = true;
        _dbContext.SaveChanges();
    }

    public IEnumerable<SessionDto> GetSessionByUserIdByAccount(ClaimsPrincipal user, string? order = null, string? orderBy = null)
    {
        var userId = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("userId is null");

        var sessions = _dbContext.Sessions.Where(s => s.UserId == int.Parse(userId) && !s.Deleted).Include(x => x.User).Include(x => x.SessionType);
        if (sessions is null || !sessions.Any())
            return Enumerable.Empty<SessionDto>();
        var orderEnum = CheckOrder(order);
        var orderByEnum = CheckOrderBy(orderBy);

        return OrderSession(sessions, orderEnum, orderByEnum);
    }

    public IEnumerable<SessionDto> GetSessionByUserIdAndTypeIdByAccount(ClaimsPrincipal user, int typeId, string? order, string? orderBy)
    {
        var userId = user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("userId is null");

        var sessions = _dbContext.Sessions.Where(s => s.UserId == int.Parse(userId) && !s.Deleted).Include(x => x.User).Include(x => x.SessionType).Where(x => x.SessionTypeId == typeId);
        if (sessions is null || !sessions.Any())
            return Enumerable.Empty<SessionDto>();
        var orderEnum = CheckOrder(order);
        var orderByEnum = CheckOrderBy(orderBy);

        return OrderSession(sessions, orderEnum, orderByEnum);
    }
    #endregion

    #region Private methods 
    private static OrderBy CheckOrderBy(string? orderBy)
    {
        var orderByEnum = OrderBy.StartTime;
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            switch (orderBy.ToLower())
            {
                case "edntime":
                case "enddate":
                    orderByEnum = OrderBy.EndTime;
                    break;

                case "userid":
                case "user":
                    orderByEnum = OrderBy.UserId;
                    break;

                case "time":
                case "duration":
                    orderByEnum = OrderBy.Duration;
                    break;

                case "sessiontype":
                case "sessiontypename":
                case "type":
                    orderByEnum = OrderBy.SessiontTypeName;
                    break;
            }
        }

        return orderByEnum;
    }

    private static Order CheckOrder(string? order)
    {
        var orderEnum = Order.Asc;
        if (!string.IsNullOrWhiteSpace(order) && order.ToLower() == "desc")
            if (order.ToLower() == "desc")
                orderEnum = Order.Desc;
        return orderEnum;
    }

    private IEnumerable<SessionDto> OrderSession(IQueryable<Session> sessionsQuery, Order order, OrderBy orderBy)
    {
        if (sessionsQuery is null || !sessionsQuery.Any())
            return Enumerable.Empty<SessionDto>();

        switch (orderBy)
        {
            case OrderBy.StartTime:
                if (order == Order.Asc)
                    sessionsQuery = sessionsQuery.OrderBy(x => x.StartTime);
                else if (order == Order.Desc)
                    sessionsQuery = sessionsQuery.OrderByDescending(x => x.StartTime);
                break;
            case OrderBy.EndTime:
                if (order == Order.Asc)
                    sessionsQuery = sessionsQuery.OrderBy(x => x.EndTime);
                else if (order == Order.Desc)
                    sessionsQuery = sessionsQuery.OrderByDescending(x => x.EndTime);
                break;
            case OrderBy.Duration:
                if (order == Order.Asc)
                    sessionsQuery = sessionsQuery.OrderBy(x => x.Duration);
                else if (order == Order.Desc)
                    sessionsQuery = sessionsQuery.OrderByDescending(x => x.Duration);
                break;
            case OrderBy.SessiontTypeName:
                if (order == Order.Asc)
                    sessionsQuery = sessionsQuery.OrderBy(x => x.SessionType.Name);
                else if (order == Order.Desc)
                    sessionsQuery = sessionsQuery.OrderByDescending(x => x.SessionType.Name);
                break;
            case OrderBy.UserId:
                if (order == Order.Asc)
                    sessionsQuery = sessionsQuery.OrderBy(x => x.User.Id);
                else if (order == Order.Desc)
                    sessionsQuery = sessionsQuery.OrderByDescending(x => x.User.Id);
                break;
        }

        var result = sessionsQuery.Select(x => _mapper.Map<SessionDto>(x));
        return result;
    }
    #endregion
}