using easylogsAPI.Domain.Entities;

namespace easylogsAPI.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Userapp> Create(Userapp userapp);
    Userapp? Get(Guid userappId);
    Userapp? Get(string username);
    Userapp? GetByEmail(string email);
    List<Permission> GetPermissions(Userapp userapp);
    Task ClearPermissions(Userapp userapp);
    IQueryable<Userapp> Get();
    Task<Userapp> Update(Userapp userapp);
    Task<bool> Delete(Userapp userapp);
}