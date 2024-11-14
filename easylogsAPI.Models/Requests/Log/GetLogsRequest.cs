namespace easylogsAPI.Models.Requests.Log;

public class GetLogsRequest : BaseRequest
{
    public Guid? LogId { get; set; }
    public string? Trace { get; set; }
    public int? Type { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
}