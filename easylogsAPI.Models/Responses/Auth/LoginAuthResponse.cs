namespace easylogsAPI.Models.Responses.Auth;

public class LoginAuthResponse
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public long Expiration { get; set; }
}