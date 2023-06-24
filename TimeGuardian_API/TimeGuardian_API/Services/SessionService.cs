using AutoMapper;

using Microsoft.EntityFrameworkCore;

using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Exceptions;
using TimeGuardian_API.Models.Session;

namespace TimeGuardian_API.Services;

public interface ISessionService
{
    int Create(CreateSessionDto dto);
    void Delete(int id);
    int EndSession(EndSessionDto dto);
    IEnumerable<SessionDto> GetAll();
    SessionDto GetById(int id);
    IEnumerable<SessionDto> GetSessionByUserId(int userId, string? order = null, string? orderBy = null);
    IEnumerable<SessionDto> GetSessionByUserIdAndTypeId(int userId, int typeId, string order, string orderBy);
    SessionDto Patch(PatchSessionDto dto, int id);
    int StartSession(StartSessionDto dto);
    SessionDto Update(CreateSessionDto dto, int id);
}

public class SessionService : ISessionService
{
    private readonly ApiDbContext _dbContext;
    private readonly IMapper _mapper;

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

    public SessionService(ApiDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    #region CRUD
    public SessionDto GetById(int id)
    {
        var session = _dbContext
                        .Sessions
                        .FirstOrDefault(r => r.Id == id);

        if (session is null)
            throw new NotFoundException(NotFoundException.Entities.Session);
        else return _mapper.Map<SessionDto>(session);
    }

    public IEnumerable<SessionDto> GetAll()
    {
        var sessions = _dbContext.Sessions?.Select(session => _mapper.Map<SessionDto>(session));

        return sessions is null
            ? Enumerable.Empty<SessionDto>()
            : sessions;
    }

    public SessionDto Update(CreateSessionDto dto, int id)
    {
        var session = _dbContext
                        .Sessions
                        .FirstOrDefault(r => r.Id == id)
                        ?? throw new NotFoundException(NotFoundException.Entities.Session);

        session.StartTime = dto.StartTime;
        session.EndTime = dto.EndTime;
        if (session.EndTime.HasValue)
            session.Duration = session.EndTime.Value.Second - session.StartTime.Second;

        var user = _dbContext.Users.FirstOrDefault(x => x.Id == dto.UserId && !x.Deleted);
        if (user != null)
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
                        .FirstOrDefault(r => r.Id == id)
                        ?? throw new NotFoundException(NotFoundException.Entities.Session);

        if (dto.StartTime.HasValue)
            session.StartTime = dto.StartTime.Value;

        if (session.EndTime.HasValue)
            session.EndTime = dto.EndTime;

        if (session.EndTime.HasValue)
            session.Duration = session.EndTime.Value.Second - session.StartTime.Second;

        if (dto.UserId.HasValue)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == dto.UserId && !x.Deleted);
            if (user != null)
                session.UserId = dto.UserId.Value;
        }

        if (dto.SessionTypeId.HasValue)
        {
            var sessionType = _dbContext.SessionTypes.FirstOrDefault(x => x.Id == dto.SessionTypeId);
            if (sessionType != null)
                session.SessionTypeId = dto.SessionTypeId.Value;
        }

        _dbContext.SaveChanges();

        return _mapper.Map<SessionDto>(session);
    }

    public int Create(CreateSessionDto dto)
    {
        var session = _mapper.Map<Session>(dto);

        if (session.EndTime.HasValue)
            session.Duration = session.EndTime.Value.Second - session.StartTime.Second;

        _dbContext.Sessions.Add(session);
        _dbContext.SaveChanges();

        return session.Id;
    }

    public void Delete(int id)
    {
        var session = _dbContext
                        .Sessions
                        .FirstOrDefault(r => r.Id == id)
                        ?? throw new NotFoundException(NotFoundException.Entities.Session);

        _dbContext.Sessions.Remove(session);
        _dbContext.SaveChanges();
    }
    #endregion

    public IEnumerable<SessionDto> GetSessionByUserId(int userId, string? order = null, string? orderBy = null)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId && !x.Deleted)
            ?? throw new NotFoundException(NotFoundException.Entities.User);

        var sessions = _dbContext.Sessions.Where(s => s.UserId == userId).Include(x => x.User).Include(x => x.SessionType);
        if (sessions is null || !sessions.Any())
            return Enumerable.Empty<SessionDto>();
        var orderEnum = CheckOrder(order);
        var orderByEnum = CheckOrderBy(orderBy);

        return OrderSession(sessions, orderEnum, orderByEnum);
    }

    public IEnumerable<SessionDto> GetSessionByUserIdAndTypeId(int userId, int typeId, string order, string orderBy)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == userId && !x.Deleted)
            ?? throw new NotFoundException(NotFoundException.Entities.User);

        var sessions = _dbContext.Sessions.Where(s => s.UserId == userId).Include(x => x.User).Include(x => x.SessionType).Where(x => x.SessionTypeId == typeId);
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
        return session.Id;
    }

    public int EndSession(EndSessionDto dto)
    {
        dto.EndTime ??= DateTime.Now;

        var session = _dbContext.Sessions.Include(x => x.SessionType).FirstOrDefault(x => x.Id == dto.SessionTypeId)
            ?? throw new NotFoundException(NotFoundException.Entities.Session);

        session.EndTime = dto.EndTime;
        session.Duration = session.EndTime.Value.Second - session.StartTime.Second;

        _dbContext.SaveChanges();

        return session.Id;
    }

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
}

