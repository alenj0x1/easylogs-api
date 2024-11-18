using System.Reflection;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Shared.Consts;

namespace easylogsAPI.WebApi.Middlewares;

public class ApiKeyMiddleware(ITokenRepository tokenRepository, ILogger<ApiKeyMiddleware> logger) : IMiddleware
{
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    private ILogger<ApiKeyMiddleware> _logger = logger;

    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            if (!ApiKeyConsts.AllowedPaths.Contains(context.Request.Path.Value)) return next(context);
            
            var apiKey = context.Request.Headers["x-api-key"].FirstOrDefault() ?? throw new Exception("Missing API Key");
            var acc = _tokenRepository.GetTokenAccess(apiKey) ?? throw new Exception("Invalid API Key");

            if (!acc.IsApiKey) throw new Exception("Invalid API Key");
            
            context.Request.Headers.Authorization = $"Bearer {acc.Value}";
            return next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}