﻿using AutoMapper;

using TimeGuardian_API.Entities;
using TimeGuardian_API.Models;
using TimeGuardian_API.Models.Role;
using TimeGuardian_API.Models.SessionType;
using TimeGuardian_API.Models.User;

namespace TimeGuardian_API;

public class TimeGuardian_APIMappingProfile : Profile
{
    public TimeGuardian_APIMappingProfile()
    {
        #region User
        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>()
            .ForMember(m => m.RoleId, x => x.MapFrom(r => r.Role.Id))
            .ForMember(m => m.RoleName, x => x.MapFrom(r => r.Role.Name));

        CreateMap<User, CreateUserDto>();
        CreateMap<CreateUserDto, User>();
        #endregion

        #region SessionType
        CreateMap<SessionTypeDto, SessionType>();
        CreateMap<SessionType, SessionTypeDto>();

        CreateMap<SessionType, CreateSessionTypeDto>();
        CreateMap<CreateSessionTypeDto, SessionType>();
        #endregion

        #region Role
        CreateMap<RoleDto, Role>();
        CreateMap<Role, RoleDto>();

        CreateMap<Role, CreateRoleDto>();
        CreateMap<CreateRoleDto, Role>();
        #endregion
    }
}