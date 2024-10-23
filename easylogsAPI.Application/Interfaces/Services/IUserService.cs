using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;

namespace easylogsAPI.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserDto> Create(CreateUserRequest request);
    UserDto? Get(Guid userappId);
    List<UserDto> Get();
    Task<UserDto> Update(UpdateUserRequest request);
    Task<bool> Delete(Guid id);
}