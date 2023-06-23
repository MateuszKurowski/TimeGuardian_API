using System.ComponentModel.DataAnnotations;

using TimeGuardian_API.Entities;

namespace TimeGuardian_API.Models;

public class CreateUserDto
{
    public string Email { get; set; }

    public string Password{ get; set; }

    public string ConfirmPassword{ get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Nationality { get; set; }


    public int RoleId { get; set; } = 2;
}