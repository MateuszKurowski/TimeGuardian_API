using TimeGuardian_API.Models.User;

namespace TimeGuardian_API.Entities;

public class Session
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? Duration { get; set; }

    public bool Deleted { get; set; } = false;


    public int UserId { get; set; }

    public virtual UserDto User { get; set; }


    public int SessionTypeId { get; set; }

    public virtual SessionType SessionType { get; set; }

}