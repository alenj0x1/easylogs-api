using System.Reflection;
using AutoMapper;
using easylogsAPI.Application.Helpers;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Dto;
using easylogsAPI.Models.Responses;
using Microsoft.Extensions.Logging;

namespace easylogsAPI.Application.Services;

public class AppService(IAppRepository appRepository, IMapper mapper, ILogger<IAppService> logger) : IAppService
{
    private readonly IAppRepository _appRepository = appRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<IAppService> _logger = logger;
    private readonly ServiceData _serviceData = new ServiceData();
    
    public BaseResponse<List<PermissionDto>> GetPermissions()
    {
        try
        {
            var dt = _appRepository.GetPermissions();
            var mp = _mapper.Map<List<PermissionDto>>(dt);

            return _serviceData.CreateResponse(mp);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public BaseResponse<PermissionDto> GetPermission(int permissionId)
    {
        try
        {
            var dt = _appRepository.GetPermission(permissionId);
            var mp = _mapper.Map<PermissionDto>(dt);
            
            return _serviceData.CreateResponse(mp);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public BaseResponse<List<LogTypeDto>> GetLogtypes()
    {
        try
        {
            var dt = _appRepository.GetLogtypes();
            var mp = _mapper.Map<List<LogTypeDto>>(dt);

            return _serviceData.CreateResponse(mp);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public BaseResponse<LogTypeDto> GetLogtype(int logtypeId)
    {
        try
        {
            var dt = _appRepository.GetPermission(logtypeId);
            var mp = _mapper.Map<LogTypeDto>(dt);

            return _serviceData.CreateResponse(mp);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}