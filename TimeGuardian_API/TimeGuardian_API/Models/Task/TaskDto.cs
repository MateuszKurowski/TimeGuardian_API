using TimeGuardian_API.Entities;

namespace TimeGuardian_API.Models.Task;

public class TaskDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; } = null;

    public DateTime CreateDate { get; set; } = DateTime.Now;

    public bool IsCompleted { get; set; } = false;


    public int UserId { get; set; }
}
