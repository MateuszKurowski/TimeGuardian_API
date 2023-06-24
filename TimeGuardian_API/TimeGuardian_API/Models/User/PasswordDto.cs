namespace TimeGuardian_API.Models.User;

public class PasswordDto
{
    public string CurrentPassword { get; set; }

    public string NewPassword { get; set; }

    public string ConfirmNewPassword { get; set; }
}
