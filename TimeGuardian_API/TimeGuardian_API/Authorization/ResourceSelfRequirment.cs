using Microsoft.AspNetCore.Authorization;

namespace TimeGuardian_API.Authorization;

public class ResourceSelfRequirment : IAuthorizationRequirement
{
    public string AdminRoleName { get; }

    public ResourceSelfRequirment()
    {
        AdminRoleName = "Admin";
    }
}
