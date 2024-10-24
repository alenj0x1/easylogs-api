using System.Reflection;
using easylogsAPI.Domain.Context;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace easylogsAPI.Domain.Repositories;

public class UserRepository(EasylogsDbContext easylogsDbContext, ILogger<IAppRepository> logger) : IUserRepository
{
    private readonly EasylogsDbContext _ctx = easylogsDbContext;
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
            return _ctx.Userapps.FirstOrDefault(usra => usra.UserAppId == userappId);
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
            return _ctx.Userapps.FirstOrDefault(usra => usra.Username == username);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public List<Userapp> Get()
    {
        try
        {
            return [.. _ctx.Userapps];
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
            _ctx.Userapps.Remove(userapp);
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