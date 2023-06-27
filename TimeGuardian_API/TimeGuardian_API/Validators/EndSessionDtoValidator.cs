using FluentValidation;

using TimeGuardian_API.Data;
using TimeGuardian_API.Models.Session;

namespace TimeGuardian_API.Validators;

public class EndSessionDtoValidator : AbstractValidator<EndSessionDto>
{
    public EndSessionDtoValidator(ApiDbContext dbContext)
    {

    }
}