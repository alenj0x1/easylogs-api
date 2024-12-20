﻿namespace easylogsAPI.Dto;

public class UserAppMeDto
{
    public Guid UserAppId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<PermissionDto> Permissions { get; set; } = [];
    public SessionTypeDto SessionType { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}