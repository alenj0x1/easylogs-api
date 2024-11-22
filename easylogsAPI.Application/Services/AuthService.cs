using System.Reflection;
using AutoMapper;
using easylogsAPI.Application.Helpers;
using easylogsAPI.Application.Interfaces.Helpers;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.Auth;
using easylogsAPI.Models.Responses;
using easylogsAPI.Models.Responses.Auth;
using easylogsAPI.Shared;
using easylogsAPI.Shared.Consts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace easylogsAPI.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IConfiguration _config;
    private readonly IToken _tokenHelper;
    private readonly ILogger<IAuthService> _logger;
    private readonly IMapper _mappper;
    private readonly ServiceData _serviceData = new ();

    public AuthService(IUserRepository userRepository, ITokenRepository tokenRepository, IConfiguration configuration, ILogger<IAuthService> logger, IMapper mapper)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _config = configuration;
        _tokenHelper = new Token(_config);
        _logger = logger;
        _mappper = mapper;
    }

    public async Task<BaseResponse<LoginAuthResponse>> CreateAccessToken(HttpContext httpContext, CreateAccessTokenAuthRequest request)
    {
        try
        {
            var usr = _userRepository.Get(request.UserAppId) ?? throw new Exception("user id not found");
            var expiration = Parser.ToDateTime(request.Expiration ?? "") ?? DateTime.MaxValue.ToUniversalTime();
            
            return _serviceData.CreateResponse(await CreateTokens(usr, expiration: expiration), "Token access created correctly");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public BaseResponse<string> ValidateToken()
    {
        try
        {
            return _serviceData.CreateResponse("validation");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public BaseResponse<List<TokenAccessDto>> GetTokenAccesses(BaseRequest request)
    {
        try
        {
            var dt = _tokenRepository.GetTokenAccesses().Skip(request.Offset).Take(request.Limit).ToList();

            List<TokenAccessDto> mp = [];
            foreach (var tkad in dt)
            {
                var tkadMp = _mappper.Map<TokenAccessDto>(tkad);
                var usr = _userRepository.Get(tkad.UserAppId);
                
                if (usr is null) continue;
                
                var perms = _userRepository.GetPermissions(usr);

                tkadMp.Value =  "*******************" + tkadMp.Value[^10..];
                tkadMp.UserApp = _mappper.Map<UserAppDefaultDto>(usr);
                tkadMp.UserApp.Permissions = _mappper.Map<List<PermissionDto>>(perms);
                
                mp.Add(tkadMp);
            }
            
            return _serviceData.CreateResponse(mp, count: _tokenRepository.GetTokenAccesses().Count(tka => tka.IsApiKey));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<BaseResponse<bool>> RemoveAccessToken(int id)
    {
        try
        {
            var acc = _tokenRepository.GetTokenAccess(id) ?? throw new Exception("Token access not found");
            var tkr = _tokenRepository.GetTokenRefresh(acc.TokenRefreshId) ?? throw new Exception("Token access not found");
            
            await _tokenRepository.DeleteTokenRefresh(tkr);
            return _serviceData.CreateResponse(true, "Token access removed correctly");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<BaseResponse<LoginAuthResponse>> Login(HttpContext httpContext, LoginAuthRequest request)
    {
        try
        {
            var usr = _userRepository.Get(request.Username) ?? throw new Exception(ResponseConsts.UserOrPasswordIncorrect);
            if (!Hasher.ComparePassword(request.Password, usr.Password)) throw new Exception(ResponseConsts.UserOrPasswordIncorrect);

            return _serviceData.CreateResponse(await CreateTokens(usr, httpContext), "Logged correctly");
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<BaseResponse<LoginAuthResponse>> RenewAccess(HttpContext httpContext, RenewAccessAuthRequest request)
    {
        try
        {
            var tkRefresh = _tokenRepository.GetTokenRefresh(request.RefreshToken) ?? throw new Exception("refresh token could not be retrieved");
            var usr = _userRepository.Get(tkRefresh.UserAppId) ?? throw new Exception("user could not be retrieved");
            
            return _serviceData.CreateResponse(await CreateTokens(usr, httpContext));
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
    
    private async Task<LoginAuthResponse> CreateTokens(Userapp appuser, HttpContext? httpContext = null, DateTime? expiration = null)
    {
        try
        {
            var rnd = new Random();
            
            // Expiration
            var expirationTime = Convert.ToInt32(_config["Jwt:Expiration"] ?? throw new Exception("jwt expiration not is defined"));
            var tkExpiration = DateTime.UtcNow.AddMinutes(rnd.Next(1, expirationTime));
            
            // Generate Access and Refresh tokens
            var tkAccess = _tokenHelper.CreateAccessToken(appuser, tkExpiration) ?? throw new Exception("token could not be created");
            var tkRefresh = Hasher.HashPassword(Generate.RandomString());
            
            // Save Refresh and Token tokens
            var svTokenRefresh = await _tokenRepository.CreateTokenRefresh(new Tokenrefresh()
            {
                Value = tkRefresh,
                UserAppId = appuser.UserAppId,
                Expiration = expiration ?? (appuser.SessionTypeId == SessionTypeConsts.Infinity ? null : DateTime.UtcNow.AddDays(appuser.SessionTime)), // <- setted :)
                IsApiKey = expiration.HasValue,
                Ip = httpContext is null ? "ignore_validation" : httpContext.Connection.RemoteIpAddress?.ToString() ?? "ignore_validation"
            });
            
            var svTokenAccess = await _tokenRepository.CreateTokenAccess(new Tokenaccess()
            {
                TokenRefreshId = svTokenRefresh.TokenRefreshId, // <- refresh token ID
                UserAppId = appuser.UserAppId,
                Value = tkAccess.Value,
                Expiration = expiration?.ToUniversalTime() ?? tkExpiration,
                IsApiKey = expiration.HasValue,
                Ip = httpContext is null ? "ignore_validation" : httpContext.Connection.RemoteIpAddress?.ToString() ?? "ignore_validation"
            });
            
            return new LoginAuthResponse()
            {
                AccessToken  = svTokenAccess.Value,
                Expiration = Generate.Milliseconds(tkAccess.Expiration),
                RefreshToken = svTokenRefresh.Value,
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}