using System.Reflection;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.Log;
using easylogsAPI.Models.Responses;
using easylogsAPI.WebApi.Attributes;
using easylogsAPI.WebApi.Interfaces.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace easylogsAPI.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class LogController(ILogService logService, ILogger<UserController> logger) : ILogController
{
    private readonly ILogService _logService = logService;
    private readonly ILogger<UserController> _logger = logger;
    
    [HttpPost("create")]
    [Permission("CREATE_LOGS")]
    [Authorize]
    public async Task<BaseResponse<LogDto>> Create([FromBody] CreateLogRequest request)
    {
        try
        {
            return await _logService.Create(request);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpGet]
    [Permission("VIEW_LOGS")]
    [Authorize]
    public BaseResponse<LogDto> Get(Guid id)
    {
        try
        {
            return _logService.Get(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpPost]
    [Permission("VIEW_LOGS")]
    [Authorize]
    public BaseResponse<List<LogDto>> Get([FromBody] GetLogsRequest request)
    {
        try
        {
            return _logService.Get(request);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpPut("update/{id:guid}")]
    [Permission("UPDATE_LOGS")]
    [Authorize]
    public async Task<BaseResponse<LogDto>> Update([FromBody] UpdateLogRequest request, Guid id)
    {
        try
        {
            return await _logService.Update(id, request);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    [HttpDelete("delete/{id:guid}")]
    [Permission("DELETE_LOGS")]
    [Authorize]
    public async Task<BaseResponse<bool>> Delete(Guid id)
    {
        try
        {
            return await _logService.Delete(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}