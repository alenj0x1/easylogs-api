using System.Reflection;
using easylogsAPI.Domain.Context;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace easylogsAPI.Domain.Repositories;

public class AppRepository(EasylogsDbContext easylogsDbContext, ILogger<IAppRepository> logger) : IAppRepository
{
    private readonly EasylogsDbContext _db = easylogsDbContext;
    private readonly ILogger<IAppRepository> _logger = logger;
    
    public List<Permission> GetPermissions()
    {
        try
        {
            return [.. _db.Permissions];
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public Permission? GetPermission(int permissionId)
    {
        try
        {
            return _db.Permissions.FirstOrDefault(per => per.PermissionId == permissionId);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public List<Logtype> GetLogtypes()
    {
        try
        {
            return [.. _db.Logtypes];
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public Logtype? GetLogtype(int logtypeId)
    {
        try
        {
            return _db.Logtypes.FirstOrDefault(per => per.LogTypeId == logtypeId);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}