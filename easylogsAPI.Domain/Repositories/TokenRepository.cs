using System.Reflection;
using easylogsAPI.Domain.Context;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace easylogsAPI.Domain.Repositories;

public class TokenRepository(EasylogsDbContext easylogsDbContext, ILogger<ITokenRepository> logger) : ITokenRepository
{
    private readonly EasylogsDbContext _ctx = easylogsDbContext;
    private readonly ILogger<ITokenRepository> _logger = logger;
    
    public async Task<Tokenaccess> CreateTokenAccess(Tokenaccess tokenAccess)
    {
        try
        {
            var gtTokenAccess = GetTokenAccess(tokenAccess.UserAppId);
            if (gtTokenAccess is not null)
            {
                var delTokenAccess = await DeleteTokenAccess(gtTokenAccess);
                if (!delTokenAccess) throw new Exception("previously token access not was deleted");
            }
            
            await _ctx.Tokenaccesses.AddAsync(tokenAccess);
            await _ctx.SaveChangesAsync();
            
            return tokenAccess;
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<Tokenrefresh> CreateTokenRefresh(Tokenrefresh tokenRefresh)
    {
        try
        {
            var gtTokenRefresh = GetTokenRefresh(tokenRefresh.UserAppId);
            if (gtTokenRefresh is not null)
            {
                var delTokenRefresh = await DeleteTokenRefresh(gtTokenRefresh);
                if (!delTokenRefresh) throw new Exception("previously token refresh not was deleted");
            }
            
            await _ctx.Tokenrefreshes.AddAsync(tokenRefresh);
            await _ctx.SaveChangesAsync();
            
            return tokenRefresh;
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public Tokenaccess? GetTokenAccess(string value)
    {
        try
        {
            return _ctx.Tokenaccesses.FirstOrDefault(tkacc => tkacc.Value == value);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }
    
    private Tokenaccess? GetTokenAccess(Guid userId)
    {
        try
        {
            return _ctx.Tokenaccesses.FirstOrDefault(tkacc => tkacc.UserAppId == userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public Tokenrefresh? GetTokenRefresh(string value)
    {
        try
        {
            return _ctx.Tokenrefreshes.FirstOrDefault(tkrf => tkrf.Value == value);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public Tokenrefresh? GetTokenRefresh(Guid userId)
    {
        try
        {
            return _ctx.Tokenrefreshes.FirstOrDefault(tkrf => tkrf.UserAppId == userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    private async Task<bool> DeleteTokenAccess(Tokenaccess tokenAccess)
    {
        try
        {
            _ctx.Tokenaccesses.Remove(tokenAccess);
            
            await _ctx.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "{Class}:{Method}:{Message}", GetType().Name, MethodBase.GetCurrentMethod()?.Name, e.Message);
            throw;
        }
    }

    public async Task<bool> DeleteTokenRefresh(Tokenrefresh tokenrefresh)
    {
        try
        {
            _ctx.Tokenrefreshes.Remove(tokenrefresh);

            var tokenAccess = GetTokenAccess(tokenrefresh.UserAppId); // <- verify token access created
            if (tokenAccess is not null)
            {
                await DeleteTokenAccess(tokenAccess);
            }
            
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