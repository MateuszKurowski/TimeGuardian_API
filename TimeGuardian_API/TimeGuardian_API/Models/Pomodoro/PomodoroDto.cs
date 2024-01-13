namespace TimeGuardian_API.Models.Pomodoro;

public class PomodoroDto
{
    public int Id { get; set; }

    public int Duration { get; set; }

    public DateTime Date { get; set; }

    public int UserId { get; set; }
}
