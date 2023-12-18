using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Evergreen.Service.src.Shared;

namespace Evergreen.Service.src.Service;

public class ProductService : IProductService
{
    private IProductRepository _productRepo;
    private IMapper _mapper;

    private ICategoryRepository _categoryRepo;

    public ProductService(IProductRepository productRepo, IMapper mapper, ICategoryRepository categoryRepo)
    {
        _productRepo = productRepo;
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    public async Task<ProductReadDTO> CreateProductAsync(ProductCreateDTO newProduct)
    {
        var category = await _categoryRepo.GetOneByIdAsync(newProduct.CategoryId);
        if (category is null)
        {
            throw CustomException.NotFoundException("Category not found");
        }
        var product = _mapper.Map<ProductCreateDTO, Product>(newProduct);
        var result = await _productRepo.CreateOneAsync(product);
        return _mapper.Map<Product, ProductReadDTO>(result);
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var productToDelete = await _productRepo.GetOneByIdAsync(id);
        if (productToDelete != null)
        {
            return await _productRepo.DeleteOneAsync(productToDelete);            
        }
        throw CustomException.NotFoundException("Product not found");
    }

    public async Task<IEnumerable<ProductReadDTO>> GetAllProductsAsync(GetAllParams options)
    {
        var result = await _productRepo.GetAllAsync(options);
        return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDTO>>(result);
    }

    public async Task<ProductReadDTO> GetProductByIdAsync(Guid id)
    {
        var result = await _productRepo.GetOneByIdAsync(id);
        if (result != null)
        {
            return _mapper.Map<Product, ProductReadDTO>(result);
        }
        else
        {
            throw CustomException.NotFoundException("Product not found");
        }
    }

    public Task<ProductReadDTO> UpdateProductAsync(Guid id, ProductUpdateDTO updates)
    {
        throw new NotImplementedException();
    }
}