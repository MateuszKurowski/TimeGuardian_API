using FluentValidation;

using TimeGuardian_API.Data;

namespace TimeGuardian_API.Models.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator(ApiDbContext dbContext)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(e => e.Password);

    }
}
