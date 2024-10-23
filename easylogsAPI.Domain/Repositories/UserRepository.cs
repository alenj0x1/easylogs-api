using easylogsAPI.Domain.Context;
using easylogsAPI.Domain.Entities;
using easylogsAPI.Domain.Interfaces.Repositories;

namespace easylogsAPI.Domain.Repositories;

public class UserRepository(EasylogsDbContext easylogsDbContext) : IUserRepository
{
    private readonly EasylogsDbContext _db = easylogsDbContext;
    
    public async Task<Userapp> Create(Userapp userapp)
    {
        try
        {
            await _db.Userapps.AddAsync(userapp);
            await _db.SaveChangesAsync();
            
            return userapp;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Userapp? Get(Guid userappId)
    {
        try
        {
            return _db.Userapps.FirstOrDefault(usra => usra.UserAppId == userappId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public List<Userapp> Get()
    {
        try
        {
            return [.. _db.Userapps];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Userapp> Update(Userapp userapp)
    {
        try
        {
            _db.Userapps.Update(userapp);
            await _db.SaveChangesAsync();
            
            return userapp;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> Delete(Userapp userapp)
    {
        try
        {
            _db.Userapps.Remove(userapp);
            await _db.SaveChangesAsync();
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}