using System.Reflection;
using System.Security.Claims;
using AutoMapper;
using easylogsAPI.Application.Helpers;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Dto;
using easylogsAPI.Models.Requests.User;
using easylogsAPI.Models.Responses;
using easylogsAPI.Shared;
using easylogsAPI.Shared.Consts;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace easylogsAPI.Application.Services;

public class UserService(IAppRepository appRepository, IUserRepository userRepository, IUserPermissionRepository userPermissionRepository, IMapper mapper, ILogger<UserService> logger, IStringLocalizer<UserService> localizer) : IUserService
{
    private readonly IAppRepository _appRepository = appRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserPermissionRepository _userPermissionRepository = userPermissionRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<IUserService> _logger = logger;
    private readonly IStringLocalizer<UserService> _localizer = localizer;
    private readonly ServiceData _serviceData = new ServiceData();
    
    public async Task<BaseResponse<UserAppDefaultDto>> Create(Claim userIdClaim, CreateUserRequest request)
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
                    UserAppId = crt.UserAppId,
                    PermissionId = prm,
                    GivenAt = DateTime.UtcNow
                });
            }
            
            var mp = _mapper.Map<UserAppDefaultDto>(crt);
            mp.Permissions = _mapper.Map<List<PermissionDto>>(_userRepository.GetPermissions(crt));

            return _serviceData.CreateResponse(mp, _localizer["UserCreated"], 201, count: _userRepository.Get().Count());
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public BaseResponse<UserAppMeDto> Me(Claim userIdClaim)
    {
        try
        {
            var cru = _userRepository.Get(Parser.ToGuid(userIdClaim.Value)) ?? throw new Exception(_localizer["IdentityValidationFailed"]);
            var mp = _mapper.Map<UserAppMeDto>(cru);
            
            mp.Permissions = _mapper.Map<List<PermissionDto>>(_userRepository.GetPermissions(cru));
            mp.SessionType = _mapper.Map<SessionTypeDto>(_appRepository.GetSessiontype(cru.SessionTypeId));

            return _serviceData.CreateResponse(mp);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public BaseResponse<UserAppDefaultDto> Get(Guid userappId)
    {
        try
        {
            var dt = _userRepository.Get(userappId);
            var mp = _mapper.Map<UserAppDefaultDto>(dt);

            if (dt is null) return _serviceData.CreateResponse(mp);
            
            mp.Permissions = _mapper.Map<List<PermissionDto>>(_userRepository.GetPermissions(dt));
            
            return _serviceData.CreateResponse(mp);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public BaseResponse<List<UserAppDefaultDto>> Get(GetUsersRequest request)
    {
        try
        {
            var gt = _userRepository.Get();
            
            if (request.UserAppId is not null)
            {
                gt = gt.Where(usra => usra.UserAppId == request.UserAppId);
            }
            
            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                gt = gt.Where(usra => usra.Username.Contains(request.Username));
            }
            
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                gt = gt.Where(usra => usra.Email.Contains(request.Email));
            }

            if (!string.IsNullOrWhiteSpace(request.StartDate) && string.IsNullOrWhiteSpace(request.EndDate)) throw new Exception(_localizer["RequiredArgumentStartDateAndEndDate"]);
            if (!string.IsNullOrWhiteSpace(request.EndDate) && string.IsNullOrWhiteSpace(request.StartDate)) throw new Exception(_localizer["RequiredArgumentEndDateAndStartDate"]);
            
            if (!string.IsNullOrWhiteSpace(request.StartDate) && !string.IsNullOrWhiteSpace(request.EndDate))
            {
                var stdParsed = Parser.ToDateTime(request.StartDate) ?? throw new Exception(_localizer["IncorrectFormatStartDate"]);
                var endParsed = Parser.ToDateTime(request.EndDate) ?? throw new Exception(_localizer["IncorrectFormatEndDate"]);
                
                gt = gt.Where(usra => usra.CreatedAt >= stdParsed && usra.CreatedAt <= endParsed);
            }
            
            var listUsr = gt.Skip(request.Offset).Take(request.Limit).ToList();
            
            List<UserAppDefaultDto> listMp = [];
            foreach (var usr in listUsr)
            {
                var mp = _mapper.Map<UserAppDefaultDto>(usr);
                
                mp.Permissions = _mapper.Map<List<PermissionDto>>(_userRepository.GetPermissions(usr));
                
                listMp.Add(mp);
            }

            return _serviceData.CreateResponse(listMp, count: _userRepository.Get().Count());
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<BaseResponse<UserAppDefaultDto>> Update(Claim userIdClaim, UpdateUserRequest request, Guid userappId)
    {
        try
        {
            var cru = _userRepository.Get(Parser.ToGuid(userIdClaim.Value)) ?? throw new Exception(_localizer["IdentityValidationFailed"]);
            var adm = _userRepository.GetPermissions(cru).FirstOrDefault(uprm => uprm.PermissionId == PermissionConsts.Administrator);
            
            if (request.Permissions is not null && request.Permissions.Contains(1) && adm is null) throw new Exception(_localizer["OnlyAnAdministratorCanGrantAdministratorPermissions"]);
            if (request.Permissions is not null && request.Permissions.Count == 0) throw new Exception(_localizer["RequiredArgumentPermissions"]);
            if (request.Permissions is not null && _appRepository.GetPermissions().Count(prm => request.Permissions.Contains(prm.PermissionId)) < request.Permissions.Count) throw new Exception(_localizer["IncorrectArgumentPermission"]);
            
            var gt = _userRepository.Get(userappId) ?? throw new Exception(ResponseConsts.UserNotFound);
            
            if (!string.IsNullOrWhiteSpace(request.Username) && request.Username != gt.Username && _userRepository.Get(request.Username) is not null) throw new Exception(_localizer["UserUsernameRegistered"]);
            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != gt.Email && _userRepository.GetByEmail(request.Email) is not null) throw new Exception(_localizer["UserEmailRegistered"]);
            
            // Default data
            if (!string.IsNullOrWhiteSpace(request.Username)) gt.Username = request.Username ?? gt.Username;
            if (!string.IsNullOrWhiteSpace(request.Email)) gt.Email = request.Email ?? gt.Email;

            // Change password
            if (!string.IsNullOrWhiteSpace(request.PasswordNew) && string.IsNullOrWhiteSpace(request.PasswordCurrent)) throw new Exception("Password new is a required field");
            if (!string.IsNullOrWhiteSpace(request.PasswordCurrent) && string.IsNullOrWhiteSpace(request.PasswordNew)) throw new Exception("Password current is a required field");

            if (!string.IsNullOrWhiteSpace(request.PasswordNew) && !string.IsNullOrWhiteSpace(request.PasswordCurrent))
            {
                if (!Hasher.ComparePassword(request.PasswordCurrent, gt.Password)) throw new Exception("Password current is incorrect");
                gt.Password = Hasher.HashPassword(request.PasswordNew);
            }

            // Permissions
            if (request.Permissions is not null && request.Permissions.Count > 0)
            {
                await _userRepository.ClearPermissions(gt);
                
                foreach (var prm in request.Permissions)
                {
                    await _userPermissionRepository.Create(new Userapppermission
                    {
                        UserAppId = gt.UserAppId,
                        PermissionId = prm,
                        GivenAt = DateTime.UtcNow
                    });
                }
            }
            
            var upd = await _userRepository.Update(gt);
            var mp = _mapper.Map<UserAppDefaultDto>(upd);
            
            mp.Permissions = _mapper.Map<List<PermissionDto>>(_userRepository.GetPermissions(upd));
            
            return _serviceData.CreateResponse(mp, _localizer["UserUpdated"], 204, count: _userRepository.Get().Count());
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
            
            return _serviceData.CreateResponse(del, _localizer["UserDeleted"], count: _userRepository.Get().Count());
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}