using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;

namespace Evergreen.Core.src.Abstraction;

public interface IProductRepository : IBaseRepository<Product>
{
    public Task<MaxMinPrice> GetMaxMinPrice();
    Task<int> GetCountAsync(GetAllParams options);
    Task<IEnumerable<Image>> GetProductImages(Guid productId);
}