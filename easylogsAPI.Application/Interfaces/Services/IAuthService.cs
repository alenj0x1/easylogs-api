using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.Auth;
using easylogsAPI.Models.Responses;
using easylogsAPI.Models.Responses.Auth;
using Microsoft.AspNetCore.Http;

namespace easylogsAPI.Application.Interfaces.Services;

public interface IAuthService
{
    Task<BaseResponse<LoginAuthResponse>> CreateAccessToken(HttpContext httpContext, CreateAccessTokenAuthRequest request);
    BaseResponse<string> ValidateToken();
    BaseResponse<List<TokenAccessDto>> GetTokenAccesses(BaseRequest request);
    Task<BaseResponse<bool>> RemoveAccessToken(HttpContext httpContext, string accessToken);
    Task<BaseResponse<LoginAuthResponse>> Login(HttpContext httpContext, LoginAuthRequest request);
    Task<BaseResponse<LoginAuthResponse>> RenewAccess(HttpContext httpContext, RenewAccessAuthRequest request);
}