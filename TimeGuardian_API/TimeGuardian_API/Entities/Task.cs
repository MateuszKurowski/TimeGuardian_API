namespace TimeGuardian_API.Entities;

public class Task
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; } = null;

    public DateTime CreateDate { get; set; } = DateTime.Now;

    public bool IsCompleted { get; set; } = false;


    public int UserId { get; set; }

    public virtual User User { get; set; }


    public int TaskListId { get; set; }

    public virtual TaskList TaskList { get; set; }

}
