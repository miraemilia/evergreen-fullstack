using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Abstraction;
public interface IProductDetailsService : IBaseService<ProductDetails, IProductDetailsRepository, ProductDetailsReadDTO, ProductDetailsCreateDTO, ProductDetailsUpdateDTO>
{}