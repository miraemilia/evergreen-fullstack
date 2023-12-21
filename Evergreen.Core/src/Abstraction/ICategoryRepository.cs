using Evergreen.Core.src.Entity;

namespace Evergreen.Core.src.Abstraction;

public interface ICategoryRepository : IBaseRepository<Category>
{
    public Task<IEnumerable<Category>> GetAllParameterlessAsync();
}