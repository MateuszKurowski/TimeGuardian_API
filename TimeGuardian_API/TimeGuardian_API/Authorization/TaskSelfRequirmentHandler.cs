using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

using Task = TimeGuardian_API.Entities.Task;

namespace TimeGuardian_API.Authorization;

public class TaskSelfRequirmentHandler : AuthorizationHandler<TaskSelfRequirment, Task>
{
    protected override System.Threading.Tasks.Task HandleRequirementAsync(AuthorizationHandlerContext context, TaskSelfRequirment requirement, Task resource)
    {
        var userId = context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var roleName = context.User.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;

        if (resource.UserId == int.Parse(userId)
                || roleName == requirement.AdminRoleName)
            context.Succeed(requirement);

        return System.Threading.Tasks.Task.CompletedTask;
    }
}