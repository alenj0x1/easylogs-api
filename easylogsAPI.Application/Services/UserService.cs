﻿using System.Reflection;
using System.Security.Claims;
using AutoMapper;
using easylogsAPI.Application.Helpers;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.User;
using easylogsAPI.Models.Responses;
using easylogsAPI.Shared;
using easylogsAPI.Shared.Consts;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace easylogsAPI.Application.Services;

public class UserService(IAppRepository appRepository, IUserRepository userRepository, IUserPermissionRepository userPermissionRepository, ITokenRepository tokenRepository, IMapper mapper, ILogger<UserService> logger, IStringLocalizer<UserService> localizer) : IUserService
{
    private readonly IAppRepository _appRepository = appRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserPermissionRepository _userPermissionRepository = userPermissionRepository;
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<IUserService> _logger = logger;
    private readonly IStringLocalizer<UserService> _localizer = localizer;
    private readonly ServiceData _serviceData = new ServiceData();
    
    public async Task<BaseResponse<UserDto>> Create(Claim userIdClaim, CreateUserRequest request)
    {
        try
        {
            var cru = _userRepository.Get(Parser.ToGuid(userIdClaim.Value)) ?? throw new Exception(_localizer["IdentityValidationFailed"]);
            var adm = _userRepository.GetPermissions(cru).FirstOrDefault(uprm => uprm.PermissionId == PermissionConsts.Administrator);
            
            if (request.Permissions.Contains(1) && adm is null) throw new Exception(_localizer["OnlyAnAdministratorCanGrantAdministratorPermissions"]);
            if (request.Permissions.Count == 0) throw new Exception(_localizer["RequiredArgumentPermissions"]);
            if (_appRepository.GetPermissions().Count(prm => request.Permissions.Contains(prm.PermissionId)) < request.Permissions.Count) throw new Exception(_localizer["IncorrectArgumentPermission"]);
            if (_userRepository.Get(request.Username) is not null) throw new Exception(_localizer["UserUsernameRegistered"]);
            if (_userRepository.GetByEmail(request.Email) is not null) throw new Exception(_localizer["UserEmailRegistered"]);
            
            
            var crt = await _userRepository.Create(new Userapp()
            {
                Username = request.Username,
                Email = request.Email,
                Password = Hasher.HashPassword(request.Password)
            });

            foreach (var prm in request.Permissions)
            {
                await _userPermissionRepository.Create(new Userapppermission
                {
                    Userid = crt.UserAppId,
                    PermissionId = prm,
                    GivenAt = DateTime.UtcNow
                });
            }
            
            var mp = _mapper.Map<UserDto>(crt);

            return _serviceData.CreateResponse(mp, _localizer["UserCreated"], 201);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public BaseResponse<UserDto> Get(Guid userappId)
    {
        try
        {
            var dt = _userRepository.Get(userappId);
            var mp = _mapper.Map<UserDto>(dt);

            return _serviceData.CreateResponse(mp);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public BaseResponse<List<UserDto>> Get(GetUsersRequest request)
    {
        try
        {
            var gt = _userRepository.Get();
            
            if (request.UserAppId is not null)
            {
                gt = gt.Where(usra => usra.UserAppId == request.UserAppId);
            }
            
            if (request.Username is not null)
            {
                gt = gt.Where(usra => usra.Username == request.Username);
            }
            
            if (request.Email is not null)
            {
                gt = gt.Where(usra => usra.Email == request.Email);
            }

            if (request.StartDate is not null && request.EndDate is null) throw new Exception(_localizer["RequiredArgumentStartDateAndEndDate"]);
            if (request.EndDate is not null && request.StartDate is null) throw new Exception(_localizer["RequiredArgumentEndDateAndStartDate"]);
            
            if (request.StartDate is not null && request.EndDate is not null)
            {
                var stdParsed = Parser.ToDateTime(request.StartDate) ?? throw new Exception(_localizer["IncorrectFormatStartDate"]);
                var endParsed = Parser.ToDateTime(request.EndDate) ?? throw new Exception(_localizer["IncorrectFormatEndDate"]);
                
                gt = gt.Where(usra => usra.CreatedAt >= stdParsed && usra.CreatedAt <= endParsed);
            }
            
            var mp = _mapper.Map<List<UserDto>>(gt.Skip(request.Offset).Take(request.Limit).ToList());

            return _serviceData.CreateResponse(mp);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<BaseResponse<UserDto>> Update(Claim userIdClaim, UpdateUserRequest request, Guid userappId)
    {
        try
        {
            var cru = _userRepository.Get(Parser.ToGuid(userIdClaim.Value)) ?? throw new Exception(_localizer["IdentityValidationFailed"]);
            var adm = _userRepository.GetPermissions(cru).FirstOrDefault(uprm => uprm.PermissionId == PermissionConsts.Administrator);
            
            if (request.Permissions is not null && request.Permissions.Contains(1) && adm is null) throw new Exception(_localizer["OnlyAnAdministratorCanGrantAdministratorPermissions"]);
            if (request.Permissions is not null && request.Permissions.Count == 0) throw new Exception(_localizer["RequiredArgumentPermissions"]);
            if (request.Permissions is not null && _appRepository.GetPermissions().Count(prm => request.Permissions.Contains(prm.PermissionId)) < request.Permissions.Count) throw new Exception(_localizer["IncorrectArgumentPermission"]);
            if (request.Username is not null && _userRepository.Get(request.Username) is not null) throw new Exception(_localizer["UserUsernameRegistered"]);
            if (request.Email is not null && _userRepository.GetByEmail(request.Email) is not null) throw new Exception(_localizer["UserEmailRegistered"]);
            
            var gt = _userRepository.Get(userappId) ?? throw new Exception(ResponseConsts.UserNotFound);
            
            gt.Username = request.Username ?? gt.Username;
            gt.Password = request.Password is not null ? Hasher.HashPassword(request.Password) : gt.Password;
            gt.Email = request.Email ?? gt.Email;

            if (request.Permissions is not null)
            {
                var usrPerms = _userRepository.GetPermissions(gt);

                foreach (var prm in request.Permissions.Where(prm => usrPerms.All(usrp => usrp.PermissionId != prm)))
                {
                    await _userPermissionRepository.Create(new Userapppermission
                    {
                        Userid = gt.UserAppId,
                        PermissionId = prm,
                        GivenAt = DateTime.UtcNow
                    });
                }
            }
            
            var upd = await _userRepository.Update(gt);
            var mp = _mapper.Map<UserDto>(upd);

            return _serviceData.CreateResponse(mp, _localizer["UserUpdated"], 204);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<BaseResponse<bool>> Delete(Claim userIdClaim, Guid userappId)
    {
        try
        {
            var cru = _userRepository.Get(Parser.ToGuid(userIdClaim.Value)) ?? throw new Exception(_localizer["IdentityValidationFailed"]);

            if (cru.UserAppId == userappId) throw new Exception(_localizer["CannotEliminateItself"]);
            
            var gt = _userRepository.Get(userappId) ?? throw new Exception(ResponseConsts.UserNotFound);
            
            gt.DeletedAt = DateTime.UtcNow;
            var del = await _userRepository.Delete(gt);
            
            return _serviceData.CreateResponse(del, _localizer["UserDeleted"]);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}