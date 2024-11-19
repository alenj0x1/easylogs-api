using System.Reflection;
using easylogsAPI.Domain.Context;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace easylogsAPI.Domain.Repositories;

public class UserRepository(EasyLogsDbContext easylogsDbContext, ILogger<IAppRepository> logger) : IUserRepository
{
    private readonly EasyLogsDbContext _ctx = easylogsDbContext;
    private readonly ILogger<IAppRepository> _logger = logger;
    
    public async Task<Userapp> Create(Userapp userapp)
    {
        try
        {
            await _ctx.Userapps.AddAsync(userapp);
            await _ctx.SaveChangesAsync();
            
            return userapp;
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public Userapp? Get(Guid userappId)
    {
        try
        {
            return _ctx.Userapps.FirstOrDefault(usra => usra.UserAppId == userappId && usra.DeletedAt == null);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public Userapp? Get(string username)
    {
        try
        {
            return _ctx.Userapps.FirstOrDefault(usra => usra.Username == username && usra.DeletedAt == null);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public Userapp? GetByEmail(string email)
    {
        try
        {
            return _ctx.Userapps.FirstOrDefault(usra => usra.Email == email && usra.DeletedAt == null);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
    
    public List<Permission> GetPermissions(Userapp userapp)
    {
        try
        {
            var dt = _ctx.Userapppermissions
                .Where(t => t.UserAppId == userapp.UserAppId)
                .Join(_ctx.Permissions, 
                    userapppermission => userapppermission.PermissionId, 
                    permission => permission.PermissionId, 
                    (usrapp, perm) => perm);
            
            return [.. dt];
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public IQueryable<Userapp> Get()
    {
        try
        {
            return _ctx.Userapps;
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<Userapp> Update(Userapp userapp)
    {
        try
        {
            _ctx.Userapps.Update(userapp);
            await _ctx.SaveChangesAsync();
            
            return userapp;
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<bool> Delete(Userapp userapp)
    {
        try
        {
            _ctx.Userapps.Update(userapp);
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