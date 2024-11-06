using System.Reflection;
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
using Microsoft.Extensions.Logging;

namespace easylogsAPI.Application.Services;

public class UserService(IAppRepository appRepository, IUserRepository userRepository, IUserPermissionRepository userPermissionRepository, ITokenRepository tokenRepository, IMapper mapper, ILogger<IUserService> logger) : IUserService
{
    private readonly IAppRepository _appRepository = appRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserPermissionRepository _userPermissionRepository = userPermissionRepository;
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<IUserService> _logger = logger;
    private readonly ServiceData _serviceData = new ServiceData();
    
    public async Task<BaseResponse<UserDto>> Create(Claim userIdClaim, CreateUserRequest request)
    {
        try
        {
            var cru = _userRepository.Get(Parser.ToGuid(userIdClaim.Value)) ?? throw new Exception("token not found");
            var adm = _userRepository.GetPermissions(cru).FirstOrDefault(uprm => uprm.PermissionId == 1);
            
            if (request.Permissions.Contains(1) && adm is null) throw new Exception("only an administrator, can assign an administrator permission");
            if (request.Permissions.Count == 0) throw new Exception("permissions is a required argument");
            if (_appRepository.GetPermissions().Count(prm => request.Permissions.Contains(prm.PermissionId)) < request.Permissions.Count) throw new Exception("a permission argumented is invalid");
            if (_userRepository.Get(request.Username) is not null) throw new Exception("user with username already exists");
            if (_userRepository.GetByEmail(request.Email) is not null) throw new Exception("user with email already exists");
            
            
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

            return _serviceData.CreateResponse(mp, ResponseConsts.UserCreated, 201);
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

    public BaseResponse<List<UserDto>> Get(BaseRequest request)
    {
        try
        {
            var gt = _userRepository.Get().Skip(request.Offset).Take(request.Limit).ToList();
            var mp = _mapper.Map<List<UserDto>>(gt);

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
            var cru = _userRepository.Get(Parser.ToGuid(userIdClaim.Value)) ?? throw new Exception("token not found");
            var adm = _userRepository.GetPermissions(cru).FirstOrDefault(uprm => uprm.PermissionId == 1);
            
            if (request.Permissions is not null && request.Permissions.Contains(1) && adm is null) throw new Exception("only an administrator, can assign an administrator permission");
            if (request.Permissions is not null && request.Permissions.Count == 0) throw new Exception("permissions is a required argument");
            if (request.Permissions is not null && _appRepository.GetPermissions().Count(prm => request.Permissions.Contains(prm.PermissionId)) < request.Permissions.Count) throw new Exception("a permission argumented is invalid");
            if (request.Username is not null && _userRepository.Get(request.Username) is not null) throw new Exception("user with username already exists");
            if (request.Email is not null && _userRepository.GetByEmail(request.Email) is not null) throw new Exception("user with email already exists");
            
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

            return _serviceData.CreateResponse(mp, ResponseConsts.UserUpdated, 204);
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
            var cru = _userRepository.Get(Parser.ToGuid(userIdClaim.Value)) ?? throw new Exception("token not found");

            if (cru.UserAppId == userappId) throw new Exception("Cannot eliminate itself");
            
            var gt = _userRepository.Get(userappId) ?? throw new Exception(ResponseConsts.UserNotFound);
            
            gt.DeletedAt = DateTime.UtcNow;
            var del = await _userRepository.Delete(gt);
            
            return _serviceData.CreateResponse(del, ResponseConsts.UserDeleted);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}