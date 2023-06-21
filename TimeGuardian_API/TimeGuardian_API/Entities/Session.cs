namespace TimeGuardian_API.Entities;

public class Session
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? Duration { get; set; }


    public int UserId { get; set; }

    public virtual User User { get; set; }


    public int SessionTypeId { get; set; }

    public virtual SessionType SessionType { get; set; }
}