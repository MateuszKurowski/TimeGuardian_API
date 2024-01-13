namespace TimeGuardian_API.Models.Pomodoro;

public class CreatePomodoroDto
{
    public int? DurationInMinutes { get; set; }

    public DateTime? Date { get; set; }
}
