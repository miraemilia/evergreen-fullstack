using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Evergreen.Controller.src.Controller;

[ApiController]
[Route("api/v1/categories")]
public class CategoryController : ControllerBase
{
    private ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [AllowAnonymous]
    [HttpGet()]
    public async Task<ActionResult<IEnumerable<CategoryReadDTO>>> GetAll([FromQuery] GetAllParams options)
    {
        return Ok(await _categoryService.GetAllCategoriesAsync(options));
    }

    [AllowAnonymous]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<CategoryReadDTO>> GetOne([FromRoute] Guid id)
    {
        return Ok(await _categoryService.GetCategoryByIdAsync(id));
    }

    [Authorize (Roles = "Admin")]
    [HttpPost()]
    public async Task<ActionResult<CategoryReadDTO>> CreateOne([FromBody] CategoryCreateDTO categoryCreateDTO)
    {
        return CreatedAtAction(nameof(CreateOne), await _categoryService.CreateCategoryAsync(categoryCreateDTO));
    }

    [Authorize (Roles = "Admin")]
    [HttpPatch("{id:Guid}")]
    public async Task<ActionResult<CategoryReadDTO>> UpdateOne([FromRoute] Guid id, [FromBody] CategoryUpdateDTO categoryUpdateDTO)
    {
        return Ok(await _categoryService.UpdateCategoryAsync(id, categoryUpdateDTO));
    }

    [Authorize (Roles= "Admin")]
    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<bool>> DeleteOne([FromRoute] Guid id)
    {
        return Ok(await _categoryService.DeleteCategoryAsync(id));
    }

}