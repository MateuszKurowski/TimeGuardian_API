using TimeGuardian_API.Models.User;

namespace TimeGuardian_API.Models.SessionType;

public class SessionTypeDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int CreatedById { get; set; }
}