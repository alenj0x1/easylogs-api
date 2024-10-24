namespace easylogsAPI.Models.Helpers;

public class CreateAccessTokenData
{
    public string Value { get; set; } = null!;
    public DateTime Expiration { get; set; }
}