using AutoMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using TimeGuardian_API.Data;
using TimeGuardian_API.Entities;
using TimeGuardian_API.Exceptions;
using TimeGuardian_API.Models;
using TimeGuardian_API.Models.User;

namespace TimeGuardian_API.Services;

public interface IUserService
{
    int Create(CreateUserDto dto);
    void Delete(int id);
    IEnumerable<UserDto> GetAll();
    UserDto GetById(int id);
    UserDto Update(CreateUserDto dto, int id);
}

public class UserService : IUserService
{
    private readonly ApiDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(ApiDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    public IEnumerable<UserDto> GetAll()
    {
        var users = _dbContext.Users?.Where(u => !u.Deleted)?.Include(r => r.Role)?.Select(u => _mapper.Map<UserDto>(u));

        return users is null
            ? Enumerable.Empty<UserDto>()
            : users;
    }

    public UserDto GetById(int id)
    {
        var user = _dbContext.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == id && !u.Deleted);

        return user is null
            ? throw new NotFoundException(NotFoundException.Entities.User)
            : _mapper.Map<UserDto>(user);
    }

    public int Create(CreateUserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        user.CreatedAt = DateTime.Now;
        _dbContext.Add(user);
        _dbContext.SaveChanges();
        return user.Id;
    }

    public UserDto Update(CreateUserDto dto, int id)
    {
        var user = _dbContext.Users.Include(x => x.Role).FirstOrDefault(u => u.Id == id && !u.Deleted) 
            ?? throw new NotFoundException(NotFoundException.Entities.User);

        var emailIsInUse = _dbContext.Users.Any(x => x.Email == dto.Email && x.Id != id);
        if (emailIsInUse)
            throw new AlreadyExistsException(AlreadyExistsException.Entities.User, dto.Email);

        var role = _dbContext.Roles.FirstOrDefault(x => x.Id == dto.RoleId);
        if (role != null)
        {
            user.RoleId = role.Id;
            user.Role = role;
        }

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
        user.DateOfBirth = dto.DateOfBirth;
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email= dto.Email;
        user.Nationality= dto.Nationality;
        user.RoleId = dto.RoleId;
        _dbContext.SaveChanges();


        return _mapper.Map<UserDto>(user);
    }

    public void Delete(int id)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == id && !u.Deleted)
            ?? throw new NotFoundException(NotFoundException.Entities.User);

        user.Deleted = true;
        _dbContext.SaveChanges();
    }

    public void ChagneRole(int userId, int roleId)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId)
            ?? throw new NotFoundException(NotFoundException.Entities.User);

        user.RoleId = roleId;
        _dbContext.SaveChanges();
    }
}
