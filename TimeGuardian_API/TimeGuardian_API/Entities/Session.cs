namespace TimeGuardian_API.Entities;

public class Session
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; } = DateTime.Now;

    public DateTime? EndTime { get; set; } = null;

    public int? Duration { get; set; } = null;

    public bool Deleted { get; set; } = false;


    public int UserId { get; set; }

    public virtual User User { get; set; }


    public int SessionTypeId { get; set; }

    public virtual SessionType SessionType { get; set; }

}