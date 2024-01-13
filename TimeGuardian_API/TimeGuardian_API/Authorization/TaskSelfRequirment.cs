using Microsoft.AspNetCore.Authorization;

namespace TimeGuardian_API.Authorization;

public class TaskSelfRequirment : IAuthorizationRequirement
{
    public string AdminRoleName { get; }

    public TaskSelfRequirment()
    {
        AdminRoleName = "Admin";
    }
}
