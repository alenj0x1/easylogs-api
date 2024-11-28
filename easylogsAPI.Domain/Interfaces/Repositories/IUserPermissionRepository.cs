using easylogsAPI.Domain.Entities;

namespace easylogsAPI.Domain.Interfaces.Repositories;

public interface IUserPermissionRepository
{
    Task<Userapppermission> Create(Userapppermission userapppermission);
    IQueryable<Userapppermission> Get();
    Task<Userapppermission> Update(Userapppermission userapppermission);
    Task<bool> Delete(Userapppermission userapppermission);
}