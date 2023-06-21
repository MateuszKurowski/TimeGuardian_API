using AutoMapper;

using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Models;

namespace TimeGuardian_API.Services;

public interface IRoleService
{
    int Create(RoleDto sessionTypeDto);
    bool Delete(int id);
    IEnumerable<Role> GetAll();
    Role? GetById(int id);
    Role? GetByName(string name);
    Role? Update(int id, RoleDto sessionType);
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

    public Role? GetById(int id)
        => _dbContext
            .Roles
            .FirstOrDefault(st => st.Id == id);

    public Role? GetByName(string name)
        => _dbContext
            .Roles
            .FirstOrDefault(st => st.Name == name);

    public IEnumerable<Role> GetAll()
        => _dbContext
            .Roles;

    public Role? Update(int id, RoleDto roleDto)
    {
        var role = GetById(id);
        if (role is null)
            return null;

        role.Name = roleDto.Name;
        _dbContext.SaveChanges();
        return role;
    }

    public int Create(RoleDto roleDto)
    {
        var role = _mapper.Map<Role>(roleDto);
        _dbContext.Roles.Add(role);
        _dbContext.SaveChanges();

        return role.Id;
    }

    public bool Delete(int id)
    {
        var role = GetById(id);
        if (role is null)
            return false;

        _dbContext.Roles.Remove(role);
        _dbContext.SaveChanges();
        return true;
    }
}