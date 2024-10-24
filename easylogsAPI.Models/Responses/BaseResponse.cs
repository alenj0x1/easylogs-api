namespace easylogsAPI.Models.Responses;

public class BaseResponse<T>
{
    public bool Ok { get; set; }
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = null!;
    public string StatusCodeCat  { get; set; } = null!;
    public DateTime TimeStamp { get; set; } = DateTime.Now;
}