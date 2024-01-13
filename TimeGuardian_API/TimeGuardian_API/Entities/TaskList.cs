namespace TimeGuardian_API.Entities;

public class TaskList
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public int UserId { get; set; }

    public virtual User User { get; set; }

}
