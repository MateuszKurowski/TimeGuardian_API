using TimeGuardian_API.Entities;

namespace TimeGuardian_API.Models.Task;

public class CreateTaskDto
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; } = null;

    public bool? IsCompleted { get; set; } = false;
}
