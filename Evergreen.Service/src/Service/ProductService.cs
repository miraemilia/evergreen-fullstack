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
    private IImageRepository _imageRepo;

    public ProductService(IProductRepository productRepo, IMapper mapper, ICategoryRepository categoryRepo, IImageRepository imageRepo)
    {
        _productRepo = productRepo;
        _mapper = mapper;
        _categoryRepo = categoryRepo;
        _imageRepo = imageRepo;
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

    public async Task<ProductPageableReadDTO> GetAllProductsAsync(GetAllParams options)
    {
        var result = await _productRepo.GetAllAsync(options);
        var total = await _productRepo.GetCountAsync(options);
        var foundProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductReadDTO>>(result);
        int pages = (total + options.Limit -1)/options.Limit;
        var response = new ProductPageableReadDTO(){Items = foundProducts, TotalItems = total, TotalPages = pages};
        return response;
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
            if (updates.CategoryId == null)
            {
                updatedProduct.CategoryId = productToUpdate.Category.Id;
            }
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

    public async Task<ProductReadDTO> AddProductImageAsync(Guid id, ProductImageDTO addDTO)
    {
        var product = await _productRepo.GetOneByIdAsync(id);
        var image = await _imageRepo.GetOneByIdAsync(addDTO.ImageId);
        if (image is null)
        {
            throw CustomException.NotFoundException("Image not found");
        }
        if (product != null)
        {
            var productImages = product.ProductImages.Append(image).ToList();
            var updatedProduct = _mapper.Map(productImages, product);
            var updated = await _productRepo.UpdateOneAsync(updatedProduct);
            return _mapper.Map<Product, ProductReadDTO>(updated);
        }
        throw CustomException.NotFoundException("Product not found");
    }

    public async Task<ProductReadDTO> RemoveProductImageAsync(Guid productId, ProductImageDTO removeDTO)
    {
        var product = await _productRepo.GetOneByIdAsync(productId);
        var image = await _imageRepo.GetOneByIdAsync(removeDTO.ImageId);
        if (product != null)
        {
            var productImages = product.ProductImages.Where(i => i.Id != removeDTO.ImageId).ToList();
            var updatedProduct = _mapper.Map(productImages, product);
            var updated = await _productRepo.UpdateOneAsync(product);
            return _mapper.Map<Product, ProductReadDTO>(updated);
        }
        throw CustomException.NotFoundException("Product not found");
    }

    public async Task<ProductReadDTO> CreateProductImageAsync(Guid productId, ImageCreateDTO imageCreateDTO)
    {
        
        var product = await _productRepo.GetOneByIdAsync(productId);
        if (product != null)
        {
            var newImage = _mapper.Map(imageCreateDTO, new Image(){});
            var createdImage = await _imageRepo.CreateOneAsync(newImage);
            await AddProductImageAsync(productId, new ProductImageDTO(){ImageId = createdImage.Id});
            return await GetProductByIdAsync(productId);
        }
        throw CustomException.NotFoundException("Product not found");
        
    }

    public async Task<IEnumerable<ImageReadDTO>> GetImagesByProductAsync(Guid productId)
    {
        var result = await _productRepo.GetProductImages(productId);
        return _mapper.Map<IEnumerable<Image>, IEnumerable<ImageReadDTO>>(result);
    }

    public async Task<MaxMinPrice> GetMaxMinPrice()
    {
        return await _productRepo.GetMaxMinPrice();
    }
}