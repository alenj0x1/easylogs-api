namespace easylogsAPI.Models.Requests.User;

public class GetUsersRequest : BaseRequest
{
    public Guid? UserAppId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
}