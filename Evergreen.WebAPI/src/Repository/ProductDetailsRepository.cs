using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;

namespace Evergreen.WebAPI.src.Repository;

public class ProductDetailsRepository : BaseRepository<ProductDetails>, IProductDetailsRepository
{
    public ProductDetailsRepository(DatabaseContext database) : base(database)
    {}

    public async Task<ProductDetails?> GetOneByProductIdAsync(Guid productId)
    {
       return await _data.FirstOrDefaultAsync(pd => pd.ProductId == productId);
    }
    
}