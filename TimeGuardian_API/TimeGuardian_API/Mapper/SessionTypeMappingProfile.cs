using AutoMapper;

using TimeGuardian_API.Entities;
using TimeGuardian_API.Models;

namespace TimeGuardian_API.Mapper;

public class SessionTypeMappingProfile : Profile
{
    public SessionTypeMappingProfile()
    {
        CreateMap<SessionTypeDto, SessionType>();
        CreateMap<SessionType, SessionTypeDto>();
    }
}