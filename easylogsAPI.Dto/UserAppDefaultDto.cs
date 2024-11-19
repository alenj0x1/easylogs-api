namespace easylogsAPI.Dto;

public class UserAppDefaultDto
{
    public Guid UserAppId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<PermissionDto> Permissions { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}