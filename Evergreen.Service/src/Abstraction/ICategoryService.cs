using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Abstraction;

public interface ICategoryService
{
    Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync();
    Task<CategoryReadDTO> GetCategoryByIdAsync(Guid id);
    Task<CategoryReadDTO> CreateCategoryAsync(CategoryCreateDTO newCategory);
    Task<bool> DeleteCategoryAsync(Guid id);
    Task<CategoryReadDTO> UpdateCategoryAsync(Guid id, CategoryUpdateDTO updates);
}