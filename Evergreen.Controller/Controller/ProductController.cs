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

// Not working: violates foreign key constraint: category_id
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

//Not working: only adds to image table, not connecting table
    [Authorize (Roles = "Admin")]
    [HttpPatch("productimages")]
    public async Task<ActionResult<ProductReadDTO>> CreateImage([FromBody] ImageCreateDTO imageCreateDTO)
    {
        await _imageService.CreateOneAsync(imageCreateDTO);
        return Ok(await _productService.GetProductByIdAsync(imageCreateDTO.ProductId));
    } 

//Not working: how to add to many-to-many connection table?
    [Authorize (Roles = "Admin")]
    [HttpPatch("productimages/{id:Guid}")]
    public async Task<ActionResult<ProductReadDTO>> AddImage([FromRoute] Guid id, [FromBody] ProductImageAddDTO imageAddDTO)
    {
        return Ok(await _productService.AddImageToProductAsync(id, imageAddDTO));
    }

    [Authorize (Roles= "Admin")]
    [HttpDelete("images/{imageId:Guid}")]
    public async Task<ActionResult<bool>> DeleteImage([FromRoute] Guid imageId)
    {
        return Ok(await _imageService.DeleteOneAsync(imageId));
    }

//Not working: service method cannot connect to repo method GetOneByProductId
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