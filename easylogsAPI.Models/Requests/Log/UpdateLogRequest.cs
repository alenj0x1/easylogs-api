using System.ComponentModel.DataAnnotations;

namespace easylogsAPI.Models.Requests.Log;

public class UpdateLogRequest
{
    public string? Message { get; set; }
    public string? Trace { get; set; }
    public string? Exception { get; set; }
    public string? StackTrace { get; set; }
    public int? Type { get; set; }
    public string? DataJson { get; set; }
}