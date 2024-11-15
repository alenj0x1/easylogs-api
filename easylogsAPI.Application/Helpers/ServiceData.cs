using easylogsAPI.Models.Responses;

namespace easylogsAPI.Application.Helpers;

public class ServiceData
{
    private const string DefaultMessage = "success request";
    private const int DefaultStatusCode = 200;
    private static readonly List<int> SuccessStatusCodes = [200, 201, 204];

    public BaseResponse<T> CreateResponse<T>(T data, string? message = DefaultMessage, int? statusCode = DefaultStatusCode, int? count = 0)
    {
        var status = statusCode ?? DefaultStatusCode;
        var crtResponse = new BaseResponse<T>()
        {
            Ok = statusCode is null || SuccessStatusCodes.Contains(statusCode.Value),
            Message = message ?? DefaultMessage,
            StatusCode = status,
            StatusCodeCat = $"https://http.cat/{status}",
            Data = data,
            Count = count ?? 0
        };
        
        return crtResponse;
    }
}