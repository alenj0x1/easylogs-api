using System.Reflection;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Dto;
using easylogsAPI.Models.Requests.User;
using easylogsAPI.Models.Responses;
using easylogsAPI.WebApi.Interfaces.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace easylogsAPI.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController(IUserService userService, ILogger<UserController> logger) : ControllerBase, IUserController
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UserController> _logger = logger;
    
    [HttpPost("create")]
    public Task<BaseResponse<UserDto>> Create(CreateUserRequest request)
    {
        try
        {
            return _userService.Create(request);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpGet("{userappId:guid}")]
    public BaseResponse<UserDto> Get(Guid userappId)
    {
        try
        {
            return _userService.Get(userappId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpGet]
    public BaseResponse<List<UserDto>> Get()
    {
        try
        {
            return _userService.Get();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpPut("update/{userappId:guid}")]
    public Task<BaseResponse<UserDto>> Update([FromBody] UpdateUserRequest request, Guid userappId)
    {
        try
        {
            return _userService.Update(request, userappId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpDelete("delete/{userappId:guid}")]
    public Task<BaseResponse<bool>> Delete(Guid userappId)
    {
        try
        {
            return _userService.Delete(userappId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}