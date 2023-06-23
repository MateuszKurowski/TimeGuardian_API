using System.ComponentModel.DataAnnotations;

namespace TimeGuardian_API.Models;

public class SessionDto : SessionUpdateDto
{
    [Required]
    public int UserId { get; set; }
}