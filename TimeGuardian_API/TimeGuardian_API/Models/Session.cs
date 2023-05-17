namespace TimeGuardian_API.Models;

public class Session
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int SessionTypeId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? Duration { get; set; }

    public User User { get; set; }
    public SessionType SessionType { get; set; }
}