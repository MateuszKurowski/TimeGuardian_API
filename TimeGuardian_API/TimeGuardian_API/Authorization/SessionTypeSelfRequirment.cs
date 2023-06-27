using Microsoft.AspNetCore.Authorization;

namespace TimeGuardian_API.Authorization;

public class SessionTypeSelfRequirment : IAuthorizationRequirement
{
    public string AdminRoleName { get; }

    public SessionTypeSelfRequirment()
    {
        AdminRoleName = "Admin";
    }
}
