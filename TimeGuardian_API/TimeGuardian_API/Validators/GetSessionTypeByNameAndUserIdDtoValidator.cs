using FluentValidation;

using TimeGuardian_API.Data;
using TimeGuardian_API.Models.Role;
using TimeGuardian_API.Models.SessionType;

namespace TimeGuardian_API.Validators;

public class GetSessionTypeByNameAndUserIdDtoValidator : AbstractValidator<GetSessionTypeByNameAndUserIdDto>
{
    public GetSessionTypeByNameAndUserIdDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
