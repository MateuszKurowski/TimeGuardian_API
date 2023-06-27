using FluentValidation;

using TimeGuardian_API.Data;
using TimeGuardian_API.Models.Role;

namespace TimeGuardian_API.Validators;

public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
{
    public CreateRoleDtoValidator(ApiDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Custom((value, context) =>
            {
                var emailExists = dbContext.Roles.Any(u => u.Name == value);
                if (emailExists)
                {
                    context.AddFailure("Name", $"Role with name: '{value}' already exists.");
                }
            });

    }
}
