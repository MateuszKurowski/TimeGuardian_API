using System.ComponentModel.DataAnnotations;

namespace TimeGuardian_API.Models;

public class UserDTO
{
    public string Email { get; set; }

    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }
}