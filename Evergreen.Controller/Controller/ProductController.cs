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

    public ProductController(IProductService productService)
    {
        _productService = productService;
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

    [Authorize (Roles= "Admin")]
    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<bool>> DeleteOne([FromRoute] Guid id)
    {
        return Ok(await _productService.DeleteProductAsync(id));
    }

}