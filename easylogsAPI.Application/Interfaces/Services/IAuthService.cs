using easylogsAPI.Models.Requests.Auth;
using easylogsAPI.Models.Responses;
using easylogsAPI.Models.Responses.Auth;
using Microsoft.AspNetCore.Http;

namespace easylogsAPI.Application.Interfaces.Services;

public interface IAuthService
{
    Task<BaseResponse<LoginAuthResponse>> Login(HttpContext httpContext, LoginAuthRequest request);
    Task<BaseResponse<LoginAuthResponse>> RenewAccess(HttpContext httpContext, RenewAccessAuthRequest request);
}