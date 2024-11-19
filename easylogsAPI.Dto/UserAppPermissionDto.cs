namespace easylogsAPI.Dto;

public class UserAppPermissionDto
{
    public int PermissionId { get; set; }
    public string ShowName { get; set; } = null!;
    public DateTime GivenAt { get; set; }
}