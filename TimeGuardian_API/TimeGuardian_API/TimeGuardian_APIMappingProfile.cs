using AutoMapper;

using TimeGuardian_API.Entities;
using TimeGuardian_API.Models;
using TimeGuardian_API.Models.Pomodoro;
using TimeGuardian_API.Models.Role;
using TimeGuardian_API.Models.Session;
using TimeGuardian_API.Models.SessionType;
using TimeGuardian_API.Models.Task;
using TimeGuardian_API.Models.User;

using Task = TimeGuardian_API.Entities.Task;

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

        CreateMap<CreateSessionTypeDtoByAccount, SessionType>();
        CreateMap<SessionType, CreateSessionTypeDtoByAccount>();

        CreateMap<SessionType, GetSessionTypeByNameAndUserIdDto>();
        CreateMap<GetSessionTypeByNameAndUserIdDto, SessionType>();
        #endregion

        #region Role
        CreateMap<RoleDto, Role>();
        CreateMap<Role, RoleDto>();

        CreateMap<Role, CreateRoleDto>();
        CreateMap<CreateRoleDto, Role>();
        #endregion

        #region Session
        CreateMap<SessionDto, Session>();
        CreateMap<Session, SessionDto>();

        CreateMap<Session, CreateSessionDto>();
        CreateMap<CreateSessionDto, Session>();

        CreateMap<CreateSessionDtoByAccount, Session>();
        CreateMap<Session, CreateSessionDtoByAccount>();

        CreateMap<Session, PatchSessionDtoByAccount>();
        CreateMap<PatchSessionDtoByAccount, Session>();
        #endregion

        #region Pomodoro
        CreateMap<PomodoroDto, Pomodoro>();
        CreateMap<Pomodoro, PomodoroDto>();

        CreateMap<Pomodoro, CreatePomodoroDto>()
            .ForMember(m => m.DurationInMinutes, x => x.MapFrom(r => r.Duration));
        CreateMap<CreatePomodoroDto, Pomodoro>()
            .ForMember(m => m.Duration, x => x.MapFrom(r => r.DurationInMinutes));

        CreateMap<Pomodoro, CreateByAdminPomodoroDto>()
            .ForMember(m => m.DurationInMinutes, x => x.MapFrom(r => r.Duration));
        CreateMap<CreateByAdminPomodoroDto, Pomodoro>()
            .ForMember(m => m.Duration, x => x.MapFrom(r => r.DurationInMinutes));
        #endregion

        #region Task
        CreateMap<TaskDto, Task>();
        CreateMap<Task, TaskDto>();

        CreateMap<Task, CreateTaskDto>();
        CreateMap<CreateTaskDto, Task>();

        CreateMap<Task, CreateByAdminTaskDto>();
        CreateMap<CreateByAdminTaskDto, Task>();
        #endregion
    }
}