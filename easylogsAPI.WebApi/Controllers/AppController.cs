using System.Reflection;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Dto;
using easylogsAPI.Models.Responses;
using easylogsAPI.WebApi.Interfaces.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace easylogsAPI.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AppController(IAppService appService, ILogger<AppController> logger) : ControllerBase, IAppController
{
    private readonly IAppService _appService = appService;
    private readonly ILogger<AppController> _logger = logger;
    
    [HttpGet("permissions")]
    public BaseResponse<List<PermissionDto>> GetPermissions()
    {
        try
        {
            return _appService.GetPermissions();;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpGet("permissions/{permissionId:int}")]
    public BaseResponse<PermissionDto> GetPermission(int permissionId)
    {
        try
        {
            return _appService.GetPermission(permissionId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpGet("logTypes")]
    public BaseResponse<List<LogTypeDto>> GetLogtypes()
    {
        try
        {
            return _appService.GetLogtypes();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpGet("logTypes/{logtypeId:int}")]
    public BaseResponse<LogTypeDto> GetLogtype(int logtypeId)
    {
        try
        {
            return _appService.GetLogtype(logtypeId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}