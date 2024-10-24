namespace easylogsAPI.Dto;

public class PermissionDto
{
    public int PermissionId { get; set; }
    public string Name { get; set; } = null!;
    public string ShowName { get; set; } = null!;
    public string Description { get; set; } = null!;
}