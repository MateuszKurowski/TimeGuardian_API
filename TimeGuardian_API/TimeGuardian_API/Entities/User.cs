﻿namespace TimeGuardian_API.Entities;

public class User
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Nationality { get; set; }

    public bool Deleted { get; set; } = false;


    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }


    public int RoleId { get; set; }

    public Role Role { get; set; }
}