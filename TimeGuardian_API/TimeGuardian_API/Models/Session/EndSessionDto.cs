namespace TimeGuardian_API.Models.Session;

public class EndSessionDto
{
    public DateTime? EndTime { get; set; }

    public int UserId { get; set; }

    public int SessionTypeId { get; set; }
}