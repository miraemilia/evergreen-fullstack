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

// violates foreign key constraint: category_id
    public async Task<ProductReadDTO> UpdateProductAsync(Guid id, ProductUpdateDTO updates)
    {
        var productToUpdate = await _productRepo.GetOneByIdAsync(id);
        if (updates.CategoryId != null)
        {
            var categoryId = (Guid)updates.CategoryId;
            var category = await _categoryRepo.GetOneByIdAsync(categoryId);
            if (category is null)
            {
                throw CustomException.NotFoundException("Category not found");
            }
        }
        if (productToUpdate != null)
        {
            var updatedProduct = _mapper.Map(updates, productToUpdate);
            var updated = await _productRepo.UpdateOneAsync(updatedProduct);
            return _mapper.Map<Product, ProductReadDTO>(updated);
        }
        else
        {
            throw CustomException.NotFoundException("Product not found");
        }
    }

    public async Task<ProductReadDTO> UpdateProductInventoryAsync(Guid id, ProductInventoryUpdateDTO updates)
    {
        var productToUpdate = await _productRepo.GetOneByIdAsync(id);
        if (productToUpdate != null)
        {
            if (productToUpdate.Inventory + updates.InventoryChange < 0)
            {
                throw CustomException.InsufficientInventory($"Cannot change product inventory by {updates.InventoryChange}: not enough products in inventory.");
            }
            productToUpdate.Inventory += updates.InventoryChange;
            var updated = await _productRepo.UpdateOneAsync(productToUpdate);
            return _mapper.Map<Product, ProductReadDTO>(updated);
        }
        else
        {
            throw CustomException.NotFoundException("Product not found");
        }
    }

// how to add to many-to-many connection table?
    public async Task<ProductReadDTO> AddImageToProductAsync(Guid id, ProductImageAddDTO addDTO)
    {
        var product = await _productRepo.GetOneByIdAsync(id);
        if (product != null)
        {
            //product.ProductImages.Add(addDTO.ImageId);
            var updated = await _productRepo.UpdateOneAsync(product);
            return _mapper.Map<Product, ProductReadDTO>(updated);
        }
        throw CustomException.NotFoundException("Product not found");

    }
}