using FluentValidation;

using TimeGuardian_API.Models.User;

namespace TimeGuardian_API.Models.Validators;

public class PasswordDtoValidator : AbstractValidator<PasswordDto>
{
    public PasswordDtoValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty();

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty()
            .Equal(x => x.NewPassword);

        RuleFor(x => x.CurrentPassword)
            .NotEmpty();
    }
}