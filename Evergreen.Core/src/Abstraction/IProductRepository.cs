using Evergreen.Core.src.Entity;

namespace Evergreen.Core.src.Abstraction;

public interface IProductRepository : IBaseRepository<Product>
{
    public Task<IEnumerable<Image>> GetProductImages(Guid productId);
}