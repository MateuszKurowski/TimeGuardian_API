using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

using System.Security.Claims;

namespace TimeGuardian_API.Authorization;

public class SelfRequirementHandler : AuthorizationHandler<SelfRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SelfRequirement requirement)
    {
        if (context.Resource is HttpContext httpContext)
        {
            var resourceId = httpContext.GetRouteValue("id")?.ToString();

            var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var roleName = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            if (userId == resourceId
                || roleName == requirement.AdminRoleName)
                context.Succeed(requirement);

            if (resourceId is null)
                context.Fail();
        }

        return Task.CompletedTask;
    }
}
