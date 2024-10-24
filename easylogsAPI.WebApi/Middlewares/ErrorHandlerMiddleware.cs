using System.Security.Claims;
using easylogsAPI.Domain.Exceptions;
using easylogsAPI.Models.Responses;
using easylogsAPI.Shared;

namespace easylogsAPI.WebApi.Middlewares;

public class ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger) : IMiddleware
{
    private readonly ILogger<ErrorHandlerMiddleware> _logger = logger;
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            _logger.LogInformation("{Method} {StatusCode} {NameIdentifier}", context.Request.Method, context.Response.StatusCode, context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await next(context);
        }
        catch (NotFoundException e)
        {
            const int status = 401;
            var rsp = new BaseResponse<string>()
            {
                Ok = false,
                StatusCode = status,
                StatusCodeCat = Parser.StatusCodeToCat(status),
                Message = e.Message
            };
      
            _logger.LogInformation("{Method} {StatusCode} {NameIdentifier}", context.Request.Method, context.Response.StatusCode, context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await context.Response.WriteAsJsonAsync(rsp);
        }
        catch (Exception e)
        {
            const int status = 500;
            var rsp = new BaseResponse<string>()
            {
                Ok = false,
                StatusCode = status,
                StatusCodeCat = Parser.StatusCodeToCat(status),
                Message = e.Message
            };
      
            _logger.LogInformation("{Method} {StatusCode} {NameIdentifier}", context.Request.Method, context.Response.StatusCode, context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await context.Response.WriteAsJsonAsync(rsp);
        }
    }
}