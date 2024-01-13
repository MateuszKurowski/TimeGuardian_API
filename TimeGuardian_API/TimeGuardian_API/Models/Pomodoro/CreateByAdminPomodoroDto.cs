namespace TimeGuardian_API.Models.Pomodoro;

public class CreateByAdminPomodoroDto
{
    public int? DurationInMinutes { get; set; }

    public DateTime? Date { get; set; }
    
    public int UserId { get; set; }
}
