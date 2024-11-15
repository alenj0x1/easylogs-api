using System.Security.Claims;
using easylogsAPI.Dto;
using easylogsAPI.Models.Requests.User;
using easylogsAPI.Models.Responses;

namespace easylogsAPI.Application.Interfaces.Services;

public interface IUserService
{
    Task<BaseResponse<UserAppDefaultDto>> Create(Claim userIdClaim, CreateUserRequest request);
    BaseResponse<UserAppMeDto> Me(Claim userIdClaim);
    BaseResponse<UserAppDefaultDto> Get(Guid userappId);
    BaseResponse<List<UserAppDefaultDto>> Get(GetUsersRequest request);
    Task<BaseResponse<UserAppDefaultDto>> Update(Claim userIdClaim, UpdateUserRequest request, Guid userappId);
    Task<BaseResponse<bool>> Delete(Claim userIdClaim, Guid userappId);
}