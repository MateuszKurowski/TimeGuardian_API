namespace TimeGuardian_API.Models.Session;

public class CreateSessionDto
{
    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? Duration { get; set; }


    public int UserId { get; set; }


    public int SessionTypeId { get; set; }
}
