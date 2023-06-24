namespace TimeGuardian_API.Models.User;

public class PatchUserDto
{
    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Nationality { get; set; }
}