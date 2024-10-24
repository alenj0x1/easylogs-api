using easylogsAPI.Models.Requests.Auth;
using easylogsAPI.Models.Responses;
using easylogsAPI.Models.Responses.Auth;
using Microsoft.AspNetCore.Mvc;

namespace easylogsAPI.WebApi.Interfaces.Controllers;

public interface IAuthController
{
    Task<BaseResponse<LoginAuthResponse>> Login([FromBody] LoginAuthRequest request);
    Task<BaseResponse<LoginAuthResponse>> RenewAccess([FromBody] RenewAccessAuthRequest request);
}