using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.WebAPI.src.Database;

namespace Evergreen.WebAPI.src.Repository;

public class ProductDetailsRepository : BaseRepository<ProductDetails>, IProductDetailsRepository
{
    public ProductDetailsRepository(DatabaseContext database) : base(database)
    {}
}