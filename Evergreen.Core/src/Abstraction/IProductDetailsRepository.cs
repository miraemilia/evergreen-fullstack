using Evergreen.Core.src.Entity;

namespace Evergreen.Core.src.Abstraction;

public interface IProductDetailsRepository : IBaseRepository<ProductDetails>
{
    public Task<ProductDetails?> GetOneByProductIdAsync(Guid productId);
}