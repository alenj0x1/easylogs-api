using System.Reflection;
using System.Security.Claims;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using easylogsAPI.Models.Responses;
using easylogsAPI.Shared;
using easylogsAPI.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace easylogsAPI.WebApi.Middlewares;

public class PermissionMiddleware(
    ILogger<ErrorHandlerMiddleware> logger,
    IUserRepository userRepository,
    IAppRepository appRepository) : IMiddleware
{
    private readonly ILogger<ErrorHandlerMiddleware> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAppRepository _appRepository = appRepository;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            var end = context.GetEndpoint() ?? throw new Exception("Without permissions");
            var attr = end.Metadata.GetMetadata<PermissionAttribute>();

            if (attr is not null)
            {
                var clm = context.User.FindFirst("UserId") ?? throw new Exception("User not found");
                var usr = _userRepository.Get(Parser.ToGuid(clm.Value)) ?? throw new Exception("User not found");
                var usrPerms = _userRepository.GetPermissions(usr);

                if (usrPerms.Any(usrp => usrp.PermissionId == 1)) // <- administrator
                {
                    await next(context);
                    return;
                }
                
                var perms = _appRepository.GetPermissions().Where(prm => attr.Values.Split(',').Contains(prm.Name)).ToList();

                List<Permission> checkPerms = [];
                foreach (var prm in perms)
                {
                    if (usrPerms.FirstOrDefault(usrp => usrp.PermissionId == prm.PermissionId) is not null)
                    {
                        checkPerms.Add(prm);
                    }
                }

                if (checkPerms.Count < perms.Count)
                {
                    throw new Exception("without required permissions");
                }
            }

            await next(context);
        }
        catch (Exception e)
        {
            const int status = 401;
            var rsp = new BaseResponse<string>()
            {
                Ok = false,
                StatusCode = status,
                StatusCodeCat = Parser.StatusCodeToCat(status),
                Message = e.Message
            };

            _logger.LogInformation("{Method} {StatusCode} {NameIdentifier}", context.Request.Method,
                context.Response.StatusCode, context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await context.Response.WriteAsJsonAsync(rsp);
        }
    }
}