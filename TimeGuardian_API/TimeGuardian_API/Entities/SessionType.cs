namespace TimeGuardian_API.Entities;

public class SessionType
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? CreatedById { get; set; }

    public virtual User CreatedBy { get; set; }

    public bool Default { get; set; }
}