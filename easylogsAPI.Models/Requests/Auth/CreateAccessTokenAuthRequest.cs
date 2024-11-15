namespace easylogsAPI.Models.Requests.Auth;

public class CreateAccessTokenAuthRequest
{
    public Guid UserAppId { get; set; }
    public string? Expiration { get; set; }
}