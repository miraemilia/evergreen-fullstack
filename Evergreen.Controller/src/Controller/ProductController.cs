using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evergreen.Controller.src.Controller;

[ApiController]
[Route("api/v1/[controller]s")]
public class ProductController : ControllerBase
{
    private IProductService _productService;
    private IProductDetailsService _detailsService;

    public ProductController(IProductService productService, IProductDetailsService detailsService)
    {
        _productService = productService;
        _detailsService = detailsService;
    }

    [AllowAnonymous]
    [HttpGet()]
    public async Task<ActionResult<ProductPageableReadDTO>> GetAll([FromQuery] GetAllParams options)
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

    [AllowAnonymous]
    [HttpGet ("price-range")]
    public async Task<ActionResult<MaxMinPrice>> GetPriceRange()
    {
        return Ok(await _productService.GetMaxMinPrice());
    }

}