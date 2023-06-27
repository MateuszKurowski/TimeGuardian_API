using Microsoft.AspNetCore.Authorization;

namespace TimeGuardian_API.Authorization;

public class SessionSelfRequirment : IAuthorizationRequirement
{
    public string AdminRoleName { get; }

    public SessionSelfRequirment()
    {
        AdminRoleName = "Admin";
    }
}
