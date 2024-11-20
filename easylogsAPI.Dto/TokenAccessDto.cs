namespace easylogsAPI.Dto;

public class TokenAccessDto
{
    public int TokenAccessId { get; set; }
    public UserAppDefaultDto UserApp { get; set; }
    public string Value { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime Expiration { get; set; }
}