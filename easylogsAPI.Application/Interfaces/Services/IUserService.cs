using System.Security.Claims;
using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.User;
using easylogsAPI.Models.Responses;

namespace easylogsAPI.Application.Interfaces.Services;

public interface IUserService
{
    Task<BaseResponse<UserAppDto>> Create(Claim userIdClaim, CreateUserRequest request);
    BaseResponse<UserAppDto> Get(Guid userappId);
    BaseResponse<List<UserAppDto>> Get(GetUsersRequest request);
    Task<BaseResponse<UserAppDto>> Update(Claim userIdClaim, UpdateUserRequest request, Guid userappId);
    Task<BaseResponse<bool>> Delete(Claim userIdClaim, Guid userappId);
}