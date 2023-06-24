namespace TimeGuardian_API.Models.Session;

public class StartSessionDto
{
    public DateTime? StartTime { get; set; }

    public int UserId { get; set; }

    public int SessionTypeId { get; set; }
}
