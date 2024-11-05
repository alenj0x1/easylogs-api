using System.Security.Claims;
using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.User;
using easylogsAPI.Models.Responses;

namespace easylogsAPI.Application.Interfaces.Services;

public interface IUserService
{
    Task<BaseResponse<UserDto>> Create(Claim userIdClaim, CreateUserRequest request);
    BaseResponse<UserDto> Get(Guid userappId);
    BaseResponse<List<UserDto>> Get();
    Task<BaseResponse<UserDto>> Update(Claim userIdClaim, UpdateUserRequest request, Guid userappId);
    Task<BaseResponse<bool>> Delete(Claim userIdClaim, Guid userappId);
}