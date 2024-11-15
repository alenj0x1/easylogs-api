using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.User;
using easylogsAPI.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace easylogsAPI.WebApi.Interfaces.Controllers;

public interface IUserController
{
    Task<BaseResponse<UserAppDefaultDto>> Create([FromBody] CreateUserRequest request);
    BaseResponse<UserAppMeDto> Me();
    BaseResponse<UserAppDefaultDto> Get([FromQuery] Guid userappId);
    BaseResponse<List<UserAppDefaultDto>> Get([FromBody] GetUsersRequest request);
    Task<BaseResponse<UserAppDefaultDto>> Update([FromBody] UpdateUserRequest request, Guid userappId);
    Task<BaseResponse<bool>> Delete(Guid userappId);
}