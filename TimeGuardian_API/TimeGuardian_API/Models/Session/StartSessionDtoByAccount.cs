namespace TimeGuardian_API.Models.Session;

public class StartSessionDtoByAccount
{
    public DateTime? StartTime { get; set; }

    public int SessionTypeId { get; set; }
}