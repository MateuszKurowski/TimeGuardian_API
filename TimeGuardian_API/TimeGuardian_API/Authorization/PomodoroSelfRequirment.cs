using Microsoft.AspNetCore.Authorization;

namespace TimeGuardian_API.Authorization;

public class PomodoroSelfRequirment : IAuthorizationRequirement
{
    public string AdminRoleName { get; }

    public PomodoroSelfRequirment()
    {
        AdminRoleName = "Admin";
    }
}
