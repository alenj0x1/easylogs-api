using System.Reflection;
using easylogsAPI.Application.Helpers;
using easylogsAPI.Application.Interfaces.Helpers;
using easylogsAPI.Application.Interfaces.Services;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
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
    private readonly ServiceData _serviceData = new ();

    public AuthService(IUserRepository userRepository, ITokenRepository tokenRepository, IConfiguration configuration, ILogger<IAuthService> logger)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _config = configuration;
        _tokenHelper = new Token(_config);
        _logger = logger;
    }

    public async Task<BaseResponse<LoginAuthResponse>> CreateAccessToken(HttpContext httpContext, CreateAccessTokenAuthRequest request)
    {
        try
        {
            var usr = _userRepository.Get(request.UserAppId) ?? throw new Exception("user id not found");
            var expiration = Parser.ToDateTime(request.Expiration ?? "") ?? DateTime.UtcNow.AddYears(99);
            
            return _serviceData.CreateResponse(await CreateTokens(usr, expiration: expiration), "Token access created correctly");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<BaseResponse<bool>> RemoveAccessToken(HttpContext httpContext, string accessToken)
    {
        try
        {
            var acc = _tokenRepository.GetTokenAccess(accessToken) ?? throw new Exception("Token access not found");
            
            await _tokenRepository.DeleteTokenRefresh(acc.TokenRefresh);
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
                IsApiKey = expiration is not null,
                Ip = httpContext is null ? "ignore_validation" : httpContext.Connection.RemoteIpAddress?.ToString() ?? "ignore_validation"
            });
            
            var svTokenAccess = await _tokenRepository.CreateTokenAccess(new Tokenaccess()
            {
                TokenRefreshId = svTokenRefresh.TokenRefreshId, // <- refresh token ID
                UserAppId = appuser.UserAppId,
                Value = tkAccess.Value,
                Expiration = tkExpiration,
                IsApiKey = expiration is not null,
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