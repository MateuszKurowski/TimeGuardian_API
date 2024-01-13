namespace TimeGuardian_API.Entities;

public class Pomodoro
{
    public int Id { get; set; }

    public int Duration { get; set; }

    public DateTime Date { get; set; }
    
    
    public int UserId { get; set; }

    public virtual User User { get; set; }
}
