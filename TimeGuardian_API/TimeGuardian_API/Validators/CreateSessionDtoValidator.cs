using FluentValidation;

using TimeGuardian_API.Data;
using TimeGuardian_API.Models.Session;

namespace TimeGuardian_API.Validators;

public class CreateSessionDtoValidator : AbstractValidator<CreateSessionDto>
{
    public CreateSessionDtoValidator(ApiDbContext dbContext)
    {
        RuleFor(x => x.StartTime)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty()
            .Custom((value, context) =>
            {
                var userExists = dbContext.Users.Any(u => u.Id == value);
                if (!userExists)
                {
                    context.AddFailure("UserId", $"User with ID: '{value}' does not exists.");
                }
            });

        RuleFor(x => x.SessionTypeId)
            .NotEmpty()
            .Custom((value, context) =>
            {
                var sessionTypeExists = dbContext.SessionTypes.Any(u => u.Id == value);
                if (!sessionTypeExists)
                {
                    context.AddFailure("SessionTypeId", $"Session type with ID: '{value}' does not exists.");
                }
            });


    }
}
