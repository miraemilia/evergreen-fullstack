using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Service;

public class ProductDetailsService : BaseService<ProductDetails, ProductDetailsReadDTO, ProductDetailsCreateDTO, ProductDetailsUpdateDTO>, IProductDetailsService
{
    public ProductDetailsService(IBaseRepository<ProductDetails> repo, IMapper mapper) : base(repo, mapper)
    {}
}