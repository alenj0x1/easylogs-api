using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.Log;
using easylogsAPI.Models.Responses;

namespace easylogsAPI.Application.Interfaces.Services;

public interface ILogService
{
    Task<BaseResponse<LogDto>> Create(CreateLogRequest request);
    BaseResponse<LogDto> Get(Guid id);
    BaseResponse<List<LogDto>> Get(GetLogsRequest request);
    Task<BaseResponse<LogDto>> Update(Guid id, UpdateLogRequest request);
    Task<BaseResponse<bool>> Delete(Guid id);
}