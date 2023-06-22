using AutoMapper;

using System.Data;

using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Exceptions;
using TimeGuardian_API.Models;

namespace TimeGuardian_API.Services;

public interface ISessionTypeService
{
    int Create(SessionTypeDto sessionTypeDto);
    void Delete(int id);
    IEnumerable<SessionTypeDto> GetAll();
    SessionTypeDto GetById(int id);
    SessionTypeDto GetByName(string name);
    SessionTypeDto Update(int id, SessionTypeDto sessionTypeDto);
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

    public SessionTypeDto GetById(int id)
    {
        var sessionType = _dbContext
                                    .SessionTypes
                                    .FirstOrDefault(st => st.Id == id);

        if (sessionType is null)
            throw new NotFoundException(NotFoundException.Entities.SessionType);
        else return _mapper.Map<SessionTypeDto>(sessionType);
    }

    public SessionTypeDto GetByName(string name)
    {
        var sessionType = _dbContext
                                    .SessionTypes
                                    .FirstOrDefault(st => st.Name == name);

        if (sessionType is null)
            throw new NotFoundException(NotFoundException.Entities.SessionType);
        else return _mapper.Map<SessionTypeDto>(sessionType);
    }

    public IEnumerable<SessionTypeDto> GetAll()
    {
        var sessionTypes = _dbContext.SessionTypes?.Select(sessionType => _mapper.Map<SessionTypeDto>(sessionType));

        return sessionTypes is null
            ? Enumerable.Empty<SessionTypeDto>()
            : sessionTypes;
    }

    public SessionTypeDto Update(int id, SessionTypeDto sessionTypeDto)
    {
        if (IsAlreadyExistWithThisName(sessionTypeDto.Name))
            throw new AlreadyExistsException(AlreadyExistsException.Entities.SessionType, sessionTypeDto.Name);

        var sessionType = GetById(id);

        sessionType.Name = sessionTypeDto.Name;
        _dbContext.SaveChanges();
        return sessionType;
    }

    public int Create(SessionTypeDto sessionTypeDto)
    {
        if (IsAlreadyExistWithThisName(sessionTypeDto.Name))
            throw new AlreadyExistsException(AlreadyExistsException.Entities.SessionType, sessionTypeDto.Name);

        var sessionType = _mapper.Map<SessionType>(sessionTypeDto);
        _dbContext.SessionTypes.Add(sessionType);
        _dbContext.SaveChanges();

        return sessionType.Id;
    }

    public void Delete(int id)
    {
        var sessiontType = _dbContext
                        .SessionTypes
                        .FirstOrDefault(st => st.Id == id)
                        ?? throw new NotFoundException(NotFoundException.Entities.SessionType);

        _dbContext.SessionTypes.Remove(sessiontType);
        _dbContext.SaveChanges();
    }

    private bool IsAlreadyExistWithThisName(string name)
    {
        var sessionType = _dbContext
                                    .SessionTypes
                                    .FirstOrDefault(st => st.Name == name);
        return sessionType is not null;
    }
}