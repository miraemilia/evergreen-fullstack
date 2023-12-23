using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Abstraction;

public interface IProductService
{
    Task<ProductPageableReadDTO> GetAllProductsAsync(GetAllParams options);
    Task<ProductReadDTO> GetProductByIdAsync(Guid id);
    Task<ProductReadDTO> CreateProductAsync(ProductCreateDTO newProduct);
    Task<bool> DeleteProductAsync(Guid id);
    Task<ProductReadDTO> UpdateProductAsync(Guid id, ProductUpdateDTO updates);
    Task<ProductReadDTO> UpdateProductInventoryAsync(Guid id, ProductInventoryUpdateDTO updates);
    Task<IEnumerable<ImageReadDTO>> GetImagesByProductAsync(Guid productId);
    Task<ProductReadDTO> AddProductImageAsync(Guid id, Guid imageId);
    Task<ProductReadDTO> RemoveProductImageAsync(Guid id, Guid imageId);
    Task<ProductReadDTO> CreateProductImageAsync(Guid id, ImageCreateDTO imageCreateDTO);
    Task<MaxMinPrice> GetMaxMinPrice();
}