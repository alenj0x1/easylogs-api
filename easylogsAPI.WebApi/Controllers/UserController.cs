using System.Reflection;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Dto;
using easylogsAPI.Models.Requests.User;
using easylogsAPI.Models.Responses;
using easylogsAPI.WebApi.Attributes;
using easylogsAPI.WebApi.Interfaces.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace easylogsAPI.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController(IUserService userService, ILogger<UserController> logger) : ControllerBase, IUserController
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UserController> _logger = logger;

    [HttpPost("create")]
    [Permission("CREATE_USERS")]
    [Authorize]
    public Task<BaseResponse<UserAppDefaultDto>> Create(CreateUserRequest request)
    {
        try
        {
            var claim = User.FindFirst("UserId") ?? throw new Exception("token not found");
            return _userService.Create(claim, request);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name,
                e.Message);
            throw;
        }
    }

    [HttpGet("me")]
    [Authorize]
    public BaseResponse<UserAppMeDto> Me()
    {
        try
        {
            var claim = User.FindFirst("UserId") ?? throw new Exception("token not found");
            return _userService.Me(claim);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name,
                e.Message);
            throw;
        }
    }

    [HttpGet("{userappId:guid}")]
    [Authorize]
    public BaseResponse<UserAppDefaultDto> Get(Guid userappId)
    {
        try
        {
            return _userService.Get(userappId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name,
                e.Message);
            throw;
        }
    }

    [HttpPost]
    [Authorize]
    public BaseResponse<List<UserAppDefaultDto>> Get([FromBody] GetUsersRequest request)
    {
        try
        {
            return _userService.Get(request);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpPut("update/{userappId:guid}")]
    [Permission("UPDATE_USERS")]
    [Authorize]
    public Task<BaseResponse<UserAppDefaultDto>> Update([FromBody] UpdateUserRequest request, Guid userappId)
    {
        try
        {
            var claim = User.FindFirst("UserId") ?? throw new Exception("token not found");
            return _userService.Update(claim, request, userappId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name,
                e.Message);
            throw;
        }
    }

    [HttpDelete("delete/{userappId:guid}")]
    [Permission("DELETE_USERS")]
    [Authorize]
    public Task<BaseResponse<bool>> Delete(Guid userappId)
    {
        try
        {
            var claim = User.FindFirst("UserId") ?? throw new Exception("token not found");
            return _userService.Delete(claim, userappId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name,
                e.Message);
            throw;
        }
    }
}