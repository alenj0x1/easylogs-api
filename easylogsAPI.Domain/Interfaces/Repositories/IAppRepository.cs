using easylogsAPI.Domain.Entities;

namespace easylogsAPI.Domain.Interfaces.Repositories;

public interface IAppRepository
{
    List<Permission> GetPermissions();
    Permission? GetPermission(int permissionId);
    List<Logtype> GetLogtypes();
    Logtype? GetLogtype(int logtypeId);
}