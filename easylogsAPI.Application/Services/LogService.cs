﻿using System.Reflection;
using AutoMapper;
using easylogsAPI.Application.Helpers;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.Log;
using easylogsAPI.Models.Responses;
using Microsoft.Extensions.Logging;

namespace easylogsAPI.Application.Services;

public class LogService(ILogRepository logRepository, IAppRepository appRepository, IMapper mapper, ILogger<ILogService> logger) : ILogService
{
    private readonly ILogRepository _logRepository = logRepository;
    private readonly IAppRepository _appRepository = appRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<ILogService> _logger = logger;
    private readonly ServiceData _serviceData = new ();
    
    public async Task<BaseResponse<LogDto>> Create(CreateLogRequest request)
    {
        try
        {
            if (_appRepository.GetLogtype(request.Type) is null) throw new ApplicationException("log type is invalid");

            var crt = await _logRepository.Create(new Log
            {
                Message = request.Message,
                Type = request.Type,
                Exception = request.Exception,
                Trace = request.Trace,
                DataJson = request.DataJson
            });
            var mp = _mapper.Map<LogDto>(crt);

            return _serviceData.CreateResponse(mp, $"log with trace: '{crt.Trace}' created successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public BaseResponse<LogDto> Get(Guid id)
    {
        try
        {
            var dt = _logRepository.Get(id);
            var mp = _mapper.Map<LogDto>(dt);

            return _serviceData.CreateResponse(mp);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public BaseResponse<List<LogDto>> Get(BaseRequest request)
    {
        try
        {
            var dt = _logRepository.Get().Skip(request.Offset).Take(request.Limit).ToList();
            var mp = _mapper.Map<List<LogDto>>(dt);

            return _serviceData.CreateResponse(mp);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<BaseResponse<LogDto>> Update(Guid id, UpdateLogRequest request)
    {
        try
        {
            var gt = _logRepository.Get(id) ?? throw new Exception("log not found");
            if (request.Type.HasValue && _appRepository.GetLogtype(request.Type.Value) is null) throw new Exception("log type is invalid");

            gt.Type = request.Type ?? gt.Type;
            gt.Trace = request.Trace ?? gt.Trace;
            gt.Message = request.Message ?? gt.Message;
            gt.Exception = request.Exception ?? gt.Exception;
            gt.DataJson = request.DataJson ?? gt.DataJson;
            gt.StackTrace = request.StackTrace ?? gt.StackTrace;
            var upd = await _logRepository.Update(gt);
            var mp = _mapper.Map<LogDto>(upd);

            return _serviceData.CreateResponse(mp, $"log with trace: '{gt.Trace}' updated successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<BaseResponse<bool>> Delete(Guid id)
    {
        try
        {
            var gt = _logRepository.Get(id) ?? throw new Exception("log not found");

            var del = await _logRepository.Delete(gt);
            if (!del) throw new Exception("log delete failed");
            
            return _serviceData.CreateResponse(del, $"log with trace: '{gt.Trace}' deleted successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}