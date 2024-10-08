﻿namespace TimeGuardian_API.Exceptions;

public class NotFoundException : Exception
{
    public enum Entities : byte
    {
        Role,
        User,
        Session,
        SessionType,
        Task,
        Pomodoro,
    }

    public NotFoundException(Entities entities) : base()
    {
        switch (entities)
        {
            case Entities.Role:
                throw new NotFoundException("Role not found.");
            case Entities.User:
                throw new NotFoundException("User not found.");
            case Entities.Session:
                throw new NotFoundException("Session not found.");
            case Entities.SessionType:
                throw new NotFoundException("Session type not found.");
            case Entities.Task:
                throw new NotFoundException("Task not found.");
            case Entities.Pomodoro:
                throw new NotFoundException("Pomodoro session not found.");
        }
    }

    public NotFoundException(string message) : base(message)
    {

    }
}