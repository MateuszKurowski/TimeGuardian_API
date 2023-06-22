namespace TimeGuardian_API.Exceptions;

public class AlreadyExistsException : Exception
{
    public enum Entities : byte
    {
        Role,
        SessionType,
    }

    public AlreadyExistsException(Entities entities, string name)
    {
        switch (entities)
        {
            case Entities.Role:
                throw new AlreadyExistsException($"Role with name {name} already exists.");
            case Entities.SessionType:
                throw new AlreadyExistsException($"SessionType with name {name} already exists.");
        }
    }

    public AlreadyExistsException(string message) : base(message)
    {

    }
}