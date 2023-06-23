using Microsoft.AspNetCore.Authorization;

namespace TimeGuardian_API.Authorization;

public class SelfRequirement : IAuthorizationRequirement
{
    public string AdminRoleName { get; }

    public SelfRequirement()
    {
        AdminRoleName = "Admin";
    }
}
