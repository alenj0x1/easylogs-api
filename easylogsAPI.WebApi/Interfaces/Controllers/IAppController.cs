using easylogsAPI.Dto;
using easylogsAPI.Models.Responses;

namespace easylogsAPI.WebApi.Interfaces.Controllers;

public interface IAppController
{
    BaseResponse<List<PermissionDto>> GetPermissions();
    BaseResponse<PermissionDto> GetPermission(int permissionId);
    BaseResponse<List<LogTypeDto>> GetLogtypes();
    BaseResponse<LogTypeDto> GetLogtype(int logtypeId);
}