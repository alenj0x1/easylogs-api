using System.Reflection;
using easylogsAPI.Domain.Context;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace easylogsAPI.Domain.Repositories;

public class UserPermissionRepository(EasyLogsDbContext easylogsDbContext, ILogger<IAppRepository> logger) : IUserPermissionRepository
{
    private readonly EasyLogsDbContext _ctx = easylogsDbContext;
    private readonly ILogger<IAppRepository> _logger = logger;
    
    public async Task<Userapppermission> Create(Userapppermission userapppermission)
    {
        try
        {
            await _ctx.Userapppermissions.AddAsync(userapppermission);
            await _ctx.SaveChangesAsync();
            return userapppermission;
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public IQueryable<Userapppermission> Get()
    {
        try
        {
            return _ctx.Userapppermissions;
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<Userapppermission> Update(Userapppermission userapppermission)
    {
        try
        { 
            _ctx.Userapppermissions.Update(userapppermission);
            await _ctx.SaveChangesAsync();
            
            return userapppermission;
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<bool> Delete(Userapppermission userapppermission)
    {
        try
        {
            _ctx.Userapppermissions.Remove(userapppermission);
            await _ctx.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
}