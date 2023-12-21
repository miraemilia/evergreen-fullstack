using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;

namespace Evergreen.Core.src.Abstraction;

public interface IProductRepository : IBaseRepository<Product>
{
    public Task<IEnumerable<Image>> GetProductImages(Guid productId);
    public Task<MaxMinPrice> GetMaxMinPrice();
}