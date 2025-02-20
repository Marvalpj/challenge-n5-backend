using Domain.Entities;

namespace Application.Interface
{
    public interface IElasticsearchRepository
    {
        Task CreateIndexIfNotExistsAsync(string indexName);
        Task<bool> Index(Permission permission);
        Task<bool> UpdateIndex(Permission permission);
        Task<Permission> GetByIdAsync(string key);
        Task<IEnumerable<Permission>> GetallAsync();
    }
}
