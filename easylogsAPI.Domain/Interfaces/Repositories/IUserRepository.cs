using easylogsAPI.Domain.Entities;

namespace easylogsAPI.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Userapp> Create(Userapp userapp);
    Userapp? Get(Guid userappId);
    Userapp? Get(string username);
    List<Userapp> Get();
    Task<Userapp> Update(Userapp userapp);
    Task<bool> Delete(Userapp userapp);
}