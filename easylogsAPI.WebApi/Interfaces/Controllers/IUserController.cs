using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.User;
using easylogsAPI.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace easylogsAPI.WebApi.Interfaces.Controllers;

public interface IUserController
{
    Task<BaseResponse<UserAppDto>> Create([FromBody] CreateUserRequest request);
    BaseResponse<UserAppDto> Get([FromQuery] Guid userappId);
    BaseResponse<List<UserAppDto>> Get([FromBody] GetUsersRequest request);
    Task<BaseResponse<UserAppDto>> Update([FromBody] UpdateUserRequest request, Guid userappId);
    Task<BaseResponse<bool>> Delete(Guid userappId);
}