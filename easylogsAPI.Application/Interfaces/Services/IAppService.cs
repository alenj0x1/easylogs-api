using easylogsAPI.Dto;
using easylogsAPI.Models.Responses;

namespace easylogsAPI.Application.Interfaces.Services;

public interface IAppService
{
    BaseResponse<List<PermissionDto>> GetPermissions();
    BaseResponse<PermissionDto> GetPermission(int permissionId);
    BaseResponse<List<LogTypeDto>> GetLogtypes();
    BaseResponse<LogTypeDto> GetLogtype(int logtypeId);
}