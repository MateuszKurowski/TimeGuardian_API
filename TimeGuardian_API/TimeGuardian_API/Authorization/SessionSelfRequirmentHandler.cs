using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

using System.Security.Claims;

using TimeGuardian_API.Entities;

namespace TimeGuardian_API.Authorization;

public class SessionSelfRequirmentHandler : AuthorizationHandler<SessionSelfRequirment, Session>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SessionSelfRequirment requirement, Session resource)
    {
        var userId = context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var roleName = context.User.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;

        if (resource.UserId == int.Parse(userId)
                || roleName == requirement.AdminRoleName)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}