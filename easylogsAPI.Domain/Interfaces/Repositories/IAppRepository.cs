using easylogsAPI.Domain.Entities;

namespace easylogsAPI.Domain.Interfaces.Repositories;

public interface IAppRepository
{
    List<Permission> GetPermissions();
    Permission? GetPermission(int permissionId);
    List<Logtype> GetLogtypes();
    Sessiontype? GetSessiontype(int sessionTypeId);
    List<Sessiontype> GetSessiontypes();
    Logtype? GetLogtype(int logtypeId);
}