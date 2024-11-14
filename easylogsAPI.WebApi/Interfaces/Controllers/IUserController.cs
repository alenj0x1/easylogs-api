using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.User;
using easylogsAPI.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace easylogsAPI.WebApi.Interfaces.Controllers;

public interface IUserController
{
    Task<BaseResponse<UserDto>> Create([FromBody] CreateUserRequest request);
    BaseResponse<UserDto> Get([FromQuery] Guid userappId);
    BaseResponse<List<UserDto>> Get([FromBody] GetUsersRequest request);
    Task<BaseResponse<UserDto>> Update([FromBody] UpdateUserRequest request, Guid userappId);
    Task<BaseResponse<bool>> Delete(Guid userappId);
}