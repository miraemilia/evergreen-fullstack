using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Evergreen.Service.src.Service;
using Evergreen.Service.src.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evergreen.Controller.src.Controller;

[ApiController]
[Route("api/v1/[controller]s")]
public class ProductController : ControllerBase
{
    private IProductService _productService;
    private IImageService _imageService;
    private IProductDetailsService _detailsService;

    public ProductController(IProductService productService, IImageService imageService, IProductDetailsService detailsService)
    {
        _productService = productService;
        _imageService = imageService;
        _detailsService = detailsService;
    }

    [AllowAnonymous]
    [HttpGet()]
    public async Task<ActionResult<IEnumerable<ProductReadDTO>>> GetAll([FromQuery] GetAllParams options)
    {
        return Ok(await _productService.GetAllProductsAsync(options));
    }

    [AllowAnonymous]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<ProductReadDTO>> GetOne([FromRoute] Guid id)
    {
        return Ok(await _productService.GetProductByIdAsync(id));
    }

    [Authorize (Roles = "Admin")]
    [HttpPost()]
    public async Task<ActionResult<ProductReadDTO>> CreateOne([FromBody] ProductCreateDTO productCreateDTO)
    {
        return CreatedAtAction(nameof(CreateOne), await _productService.CreateProductAsync(productCreateDTO));
    }

    [Authorize (Roles = "Admin")]
    [HttpPatch("{id:Guid}")]
    public async Task<ActionResult<ProductReadDTO>> UpdateOne([FromRoute] Guid id, [FromBody] ProductUpdateDTO productUpdateDTO)
    {
        return Ok(await _productService.UpdateProductAsync(id, productUpdateDTO));
    }

    [Authorize (Roles = "Admin")]
    [HttpPatch("inventory/{id:Guid}")]
    public async Task<ActionResult<ProductReadDTO>> UpdateProductInventory([FromRoute] Guid id, [FromBody] ProductInventoryUpdateDTO productUpdateDTO)
    {
        return Ok(await _productService.UpdateProductInventoryAsync(id, productUpdateDTO));
    }

    [Authorize (Roles = "Admin")]
    [HttpPatch("images/{id:Guid}")]
    public async Task<ActionResult<ImageReadDTO>> UpdateImage([FromRoute] Guid id, [FromBody] ImageUpdateDTO imageUpdateDTO)
    {
        return Ok(await _imageService.UpdateOneAsync(id, imageUpdateDTO));
    }

    [Authorize (Roles = "Admin")]
    [HttpPatch("productimages")]
    public async Task<ActionResult<ProductReadDTO>> CreateProductImage([FromBody] ImageCreateDTO imageCreateDTO)
    {
        return Ok(await _productService.CreateProductImageAsync(imageCreateDTO));
    } 

    [Authorize (Roles = "Admin")]
    [HttpPatch("productimages/{productId:Guid}")]
    public async Task<ActionResult<ProductReadDTO>> AddProductImage([FromRoute] Guid productId, [FromBody] ProductImageDTO imageAddDTO)
    {
        return Ok(await _productService.AddProductImageAsync(productId, imageAddDTO));
    }

    [Authorize (Roles= "Admin")]
    [HttpDelete("images/{imageId:Guid}")]
    public async Task<ActionResult<bool>> DeleteImage([FromRoute] Guid imageId)
    {
        return Ok(await _imageService.DeleteOneAsync(imageId));
    }

    [Authorize (Roles= "Admin")]
    [HttpDelete("productimages/{productId:Guid}")]
    public async Task<ActionResult<bool>> RemoveProductImage([FromRoute] Guid productId, [FromBody] ProductImageDTO deleteDTO)
    {
        return Ok(await _productService.RemoveProductImageAsync(productId, deleteDTO));
    }

    [Authorize (Roles = "Admin")]
    [HttpPatch("details/{productId:Guid}")]
    public async Task<ActionResult<ProductReadDTO>> UpdateProductDetails([FromRoute] Guid productId, [FromBody] ProductDetailsUpdateDTO detailsUpdateDTO)
    {
        return Ok(await _detailsService.UpdateOneAsync(productId, detailsUpdateDTO));
    }

    [Authorize (Roles= "Admin")]
    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<bool>> DeleteOne([FromRoute] Guid id)
    {
        return Ok(await _productService.DeleteProductAsync(id));
    }

}