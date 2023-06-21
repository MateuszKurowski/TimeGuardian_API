using AutoMapper;

using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Models;

namespace TimeGuardian_API.Services;

public interface ISessionTypeService
{
    int Create(SessionTypeDto sessionTypeDto);
    bool Delete(int id);
    IEnumerable<SessionType> GetAll();
    SessionType? GetById(int id);
    SessionType? GetByName(string name);
    SessionType? Update(int id, SessionTypeDto sessionTypeDto);
}

public class SessionTypeService : ISessionTypeService
{
    private readonly ApiDbContext _dbContext;
    private readonly IMapper _mapper;

    public SessionTypeService(ApiDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public SessionType? GetById(int id)
        => _dbContext
            .SessionTypes
            .FirstOrDefault(st => st.Id == id);

    public SessionType? GetByName(string name)
        => _dbContext
            .SessionTypes
            .FirstOrDefault(st => st.Name == name);

    public IEnumerable<SessionType> GetAll()
        => _dbContext
            .SessionTypes;

    public SessionType? Update(int id, SessionTypeDto sessionTypeDto)
    {
        var sessionType = GetById(id);
        if (sessionType is null)
            return null;

        sessionType.Name = sessionTypeDto.Name;
        _dbContext.SaveChanges();
        return sessionType;
    }

    public int Create(SessionTypeDto sessionTypeDto)
    {
        var sessionType = _mapper.Map<SessionType>(sessionTypeDto);
        _dbContext.SessionTypes.Add(sessionType);
        _dbContext.SaveChanges();

        return sessionType.Id;
    }

    public bool Delete(int id)
    {
        var sessionType = GetById(id);
        if (sessionType is null)
            return false;

        _dbContext.SessionTypes.Remove(sessionType);
        _dbContext.SaveChanges();
        return true;
    }

}