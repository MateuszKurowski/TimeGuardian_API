using System.ComponentModel.DataAnnotations;

namespace TimeGuardian_API.Models;

public class SessionUpdateDTO
{
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? Duration { get; set; }

    [Required]
    public int SessionTypeId { get; set; }
}