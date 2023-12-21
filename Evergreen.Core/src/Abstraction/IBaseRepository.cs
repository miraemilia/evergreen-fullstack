using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;

namespace Evergreen.Core.src.Abstraction;

public interface IBaseRepository<T> where T:BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync(GetAllParams options);
    Task<int> GetCountAsync(GetAllParams options);
    Task<T?> GetOneByIdAsync(Guid id);
    Task<T> CreateOneAsync(T createItem);
    Task<bool> DeleteOneAsync(T deleteItem);
    Task<T> UpdateOneAsync(T updateItem);
}