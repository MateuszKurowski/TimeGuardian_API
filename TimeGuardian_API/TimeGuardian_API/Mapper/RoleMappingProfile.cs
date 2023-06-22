using AutoMapper;

using TimeGuardian_API.Entities;
using TimeGuardian_API.Models;

namespace TimeGuardian_API.Mapper;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<RoleDto, Role>();
        CreateMap<Role, RoleDto>();
    }
}