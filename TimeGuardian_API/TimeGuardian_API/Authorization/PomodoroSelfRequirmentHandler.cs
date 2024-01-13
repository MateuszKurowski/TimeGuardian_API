using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

using TimeGuardian_API.Entities;

namespace TimeGuardian_API.Authorization;

public class PomodoroSelfRequirmentHandler : AuthorizationHandler<PomodoroSelfRequirment, Pomodoro>
{
    protected override System.Threading.Tasks.Task HandleRequirementAsync(AuthorizationHandlerContext context, PomodoroSelfRequirment requirement, Pomodoro resource)
    {
        var userId = context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var roleName = context.User.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;

        if (resource.UserId == int.Parse(userId)
                || roleName == requirement.AdminRoleName)
            context.Succeed(requirement);

        return System.Threading.Tasks.Task.CompletedTask;
    }
}