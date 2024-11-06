using System.Reflection;
using easylogsAPI.Domain.Context;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace easylogsAPI.Domain.Repositories;

public class LogRepository(EasylogsDbContext easylogsDbContext, ILogger<IAppRepository> logger) : ILogRepository
{
    private readonly EasylogsDbContext _ctx = easylogsDbContext;
    private readonly ILogger _logger = logger;
    
    public async Task<Log> Create(Log log)
    {
        try
        {
            await _ctx.Logs.AddAsync(log);
            await _ctx.SaveChangesAsync();

            return log;
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public Log? Get(Guid id)
    {
        try
        {
            return _ctx.Logs.FirstOrDefault(log => log.LogId == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public IQueryable<Log> Get()
    {
        try
        {
            return _ctx.Logs;
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<Log> Update(Log log)
    {
        try
        {
            _ctx.Logs.Update(log);
            await _ctx.SaveChangesAsync();

            return log;
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<bool> Delete(Log log)
    {
        try
        {
            _ctx.Logs.Remove(log);
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