using easylogsAPI.Domain.Entities;

namespace easylogsAPI.Domain.Interfaces.Repositories;

public interface ILogRepository
{
    Task<Log> Create(Log log);
    Log? Get(Guid id);
    IQueryable<Log> Get();
    Task<Log> Update(Log log);
    Task<bool> Delete(Log log);
}