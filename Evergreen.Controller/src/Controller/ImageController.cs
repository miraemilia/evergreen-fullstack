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
public class ImageController : ControllerBase
{
    private IProductService _productService;
    private IImageService _imageService;
    public ImageController(IProductService productService, IImageService imageService, IProductDetailsService detailsService)
    {
        _productService = productService;
        _imageService = imageService;
    }

    [AllowAnonymous]
    [HttpGet()]
    public async Task<ActionResult<ImagePageableReadDTO>> GetAll([FromQuery] GetAllParams options)
    {
        return Ok(await _imageService.GetAllAsync(options));
    }

    [AllowAnonymous]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<ImageReadDTO>> GetOne([FromRoute] Guid id)
    {
        return Ok(await _imageService.GetOneByIdAsync(id));
    }

    [AllowAnonymous]
    [HttpGet("product/{productId:Guid}")]
    public async Task<ActionResult<ImageReadDTO>> GetProductImages([FromRoute] Guid productId)
    {
        return Ok(await _productService.GetImagesByProductAsync(productId));
    }

    [Authorize (Roles = "Admin")]
    [HttpPost()]
    public async Task<ActionResult<ImageReadDTO>> CreateOne([FromBody] ImageCreateDTO imageCreateDTO)
    {
        return CreatedAtAction(nameof(CreateOne), await _imageService.CreateOneAsync(imageCreateDTO));
    }

    [Authorize (Roles = "Admin")]
    [HttpPatch("{id:Guid}")]
    public async Task<ActionResult<ImageReadDTO>> UpdateOne([FromRoute] Guid id, [FromBody] ImageUpdateDTO imageUpdateDTO)
    {
        return Ok(await _imageService.UpdateOneAsync(id, imageUpdateDTO));
    }

    [Authorize (Roles= "Admin")]
    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<bool>> DeleteImage([FromRoute] Guid id)
    {
        return Ok(await _imageService.DeleteOneAsync(id));
    }
}