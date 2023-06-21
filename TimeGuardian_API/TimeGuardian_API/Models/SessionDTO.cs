using System.ComponentModel.DataAnnotations;

namespace TimeGuardian_API.Models;

public class SessionDTO : SessionUpdateDTO
{
    [Required]
    public int UserId { get; set; }
}