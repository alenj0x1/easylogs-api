using System.ComponentModel.DataAnnotations;

namespace easylogsAPI.Models.Requests.Log;

public class CreateLogRequest
{
    [Required]
    public string Message { get; set; } = null!;
    [Required]
    public string Trace { get; set; } = null!;
    public string? Exception { get; set; }
    public string? StackTrace { get; set; }
    [Required] 
    public int Type { get; set; } = 1;
    public string DataJson { get; set; } = null!;
}