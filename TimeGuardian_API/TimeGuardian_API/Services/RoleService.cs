using AutoMapper;

using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Exceptions;
using TimeGuardian_API.Models;

namespace TimeGuardian_API.Services;

public interface IRoleService
{
    int Create(RoleDto roleDto);
    void Delete(int id);
    IEnumerable<RoleDto> GetAll();
    RoleDto GetById(int id);
    RoleDto GetByName(string name);
    RoleDto Update(int id, RoleDto roleDto);
}

public class RoleService : IRoleService
{
    private readonly ApiDbContext _dbContext;
    private readonly IMapper _mapper;

    public RoleService(ApiDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public RoleDto GetById(int id)
    {
        var role = _dbContext
                        .Roles
                        .FirstOrDefault(r => r.Id == id);

        if (role is null)
            throw new NotFoundException(NotFoundException.Entities.Role);
        else return _mapper.Map<RoleDto>(role);
    }


    public RoleDto GetByName(string name)
    {
        var role = _dbContext
                        .Roles
                        .FirstOrDefault(r => r.Name == name);

        if (role is null)
            throw new NotFoundException(NotFoundException.Entities.Role);
        else return _mapper.Map<RoleDto>(role);
    }

    public IEnumerable<RoleDto> GetAll()
    {
        var roles = _dbContext.Roles?.Select(role => _mapper.Map<RoleDto>(role));

        return roles is null
            ? Enumerable.Empty<RoleDto>()
            : roles;
    }

    public RoleDto Update(int id, RoleDto roleDto)
    {
        if (IsAlreadyExistWithThisName(roleDto.Name))
            throw new AlreadyExistsException(AlreadyExistsException.Entities.Role, roleDto.Name);

        var role = GetById(id);

        role.Name = roleDto.Name;
        _dbContext.SaveChanges();
        return _mapper.Map<RoleDto>(role);
    }

    public int Create(RoleDto roleDto)
    {
        if (IsAlreadyExistWithThisName(roleDto.Name))
            throw new AlreadyExistsException(AlreadyExistsException.Entities.Role, roleDto.Name);

        var role = _mapper.Map<Role>(roleDto);
        _dbContext.Roles.Add(role);
        _dbContext.SaveChanges();

        return role.Id;
    }

    public void Delete(int id)
    {
        var role = _dbContext
                        .Roles
                        .FirstOrDefault(st => st.Id == id)
                        ?? throw new NotFoundException(NotFoundException.Entities.Role);

        _dbContext.Roles.Remove(role);
        _dbContext.SaveChanges();
    }

    private bool IsAlreadyExistWithThisName(string name)
    {
        var role = _dbContext
                                    .Roles
                                    .FirstOrDefault(r => r.Name == name);
        return role is not null;
    }
}