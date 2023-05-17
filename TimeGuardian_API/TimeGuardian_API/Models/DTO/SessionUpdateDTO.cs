namespace TimeGuardian_API.Models.DTO;

public class SessionUpdateDTO
{
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? Duration { get; set; }
}