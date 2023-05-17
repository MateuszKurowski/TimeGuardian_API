using System.ComponentModel.DataAnnotations;

namespace TimeGuardian_API.Models.DTO;

public class SessionTypeDTO
{
    [Required]
    public string Name { get; set; }
}