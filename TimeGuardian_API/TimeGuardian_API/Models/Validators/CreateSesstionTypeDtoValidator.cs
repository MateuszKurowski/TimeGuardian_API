using FluentValidation;

using TimeGuardian_API.Data;
using TimeGuardian_API.Models.SessionType;

namespace TimeGuardian_API.Models.Validators;

public class CreateSesstionTypeDtoValidator : AbstractValidator<CreateSessionTypeDto>
{
    public CreateSesstionTypeDtoValidator(ApiDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Custom((value, context) =>
            {
                var emailExists = dbContext.SessionTypes.Any(u => u.Name == value);
                if (emailExists)
                {
                    context.AddFailure("Name", $"Session type with name: '{value}' already exists.");
                }
            });

    }
}
