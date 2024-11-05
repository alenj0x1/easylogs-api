namespace easylogsAPI.Dto;

public class UserDto
{
    public Guid UserAppId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}