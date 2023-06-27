using FluentValidation;

using TimeGuardian_API.Data;
using TimeGuardian_API.Models.SessionType;

namespace TimeGuardian_API.Validators;

public class CreateSesstionTypeDtoValidator : AbstractValidator<CreateSessionTypeDto>
{
    public CreateSesstionTypeDtoValidator(ApiDbContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.CreatedById)
            .NotEmpty();

    }
}
