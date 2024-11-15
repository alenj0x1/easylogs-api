namespace easylogsAPI.Dto;

public class AppInfoDto
{
    public List<PermissionDto> Permissions { get; set; } = [];
    public List<SessionTypeDto> SessionTypes { get; set; } = [];
    public List<LogTypeDto> LogTypes { get; set; } = [];
}