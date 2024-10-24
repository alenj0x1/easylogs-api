using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.User;
using easylogsAPI.Models.Responses;

namespace easylogsAPI.Application.Interfaces.Services;

public interface IUserService
{
    Task<BaseResponse<UserDto>> Create(CreateUserRequest request);
    BaseResponse<UserDto> Get(Guid userappId);
    BaseResponse<List<UserDto>> Get();
    Task<BaseResponse<UserDto>> Update(UpdateUserRequest request, Guid userappId);
    Task<BaseResponse<bool>> Delete(Guid userappId);
}