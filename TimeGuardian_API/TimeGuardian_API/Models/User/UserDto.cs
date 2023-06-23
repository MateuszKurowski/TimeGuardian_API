using TimeGuardian_API.Models.Role;

namespace TimeGuardian_API.Models.User;

public class UserDto
{
    public int Id { get; set; }

    public string PasswordHash { get; set; }

    public string Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Nationality { get; set; }

    public int? RoleId {get; set; }

    public string? RoleName {get; set; }
}