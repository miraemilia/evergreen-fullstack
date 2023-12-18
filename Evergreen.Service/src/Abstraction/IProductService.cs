using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Abstraction;

public interface IProductService
{
    Task<IEnumerable<ProductReadDTO>> GetAllProductsAsync(GetAllParams options);
    Task<ProductReadDTO> GetProductByIdAsync(Guid id);
    Task<ProductReadDTO> CreateProductAsync(ProductCreateDTO newProduct);
    Task<bool> DeleteProductAsync(Guid id);
    Task<ProductReadDTO> UpdateProductAsync(Guid id, ProductUpdateDTO updates);
}