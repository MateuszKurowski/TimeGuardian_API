﻿namespace TimeGuardian_API.Models.SessionType;

public class CreateSessionTypeDto
{
    public string Name { get; set; }
    
    public int CreatedById { get; set; }
}