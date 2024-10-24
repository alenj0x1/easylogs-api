using System.Reflection;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Models.Requests.Auth;
using easylogsAPI.Models.Responses;
using easylogsAPI.Models.Responses.Auth;
using easylogsAPI.WebApi.Interfaces.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace easylogsAPI.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IAuthService authService, ILogger<IAuthController> logger) : ControllerBase, IAuthController
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<IAuthController> _logger = logger;
    
    [HttpPost("login")]
    public async Task<BaseResponse<LoginAuthResponse>> Login([FromBody] LoginAuthRequest request)
    {
        try
        {
            return await _authService.Login(HttpContext, request);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpPost("renewAccess")]
    public async Task<BaseResponse<LoginAuthResponse>> RenewAccess([FromBody] RenewAccessAuthRequest request)
    {
        try
        {
            return await _authService.RenewAccess(HttpContext, request);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}