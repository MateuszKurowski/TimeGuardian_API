using TimeGuardian_API.Models.SessionType;
using TimeGuardian_API.Models.User;

namespace TimeGuardian_API.Models.Session;

public class SessionDto
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? Duration { get; set; }


    public virtual UserDto User { get; set; }


    public virtual SessionTypeDto SessionType { get; set; }
}
