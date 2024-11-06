using easylogsAPI.Dto;
using easylogsAPI.Models.Requests;
using easylogsAPI.Models.Requests.Log;
using easylogsAPI.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace easylogsAPI.WebApi.Interfaces.Controllers;

public interface ILogController
{
    Task<BaseResponse<LogDto>> Create(CreateLogRequest request);
    BaseResponse<LogDto> Get(Guid id);
    BaseResponse<List<LogDto>> Get([FromBody] BaseRequest request);
    Task<BaseResponse<LogDto>> Update([FromBody] UpdateLogRequest request, Guid id);
    Task<BaseResponse<bool>> Delete(Guid id);
}