using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Evergreen.Service.src.Shared;

namespace Evergreen.Service.src.Service;

public class CategoryService : ICategoryService
{
    private ICategoryRepository _categoryRepo;
    private IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepo, IMapper mapper)
    {
        _categoryRepo = categoryRepo;
        _mapper = mapper;
    }

    public async Task<CategoryReadDTO> CreateCategoryAsync(CategoryCreateDTO newCategory)
    {
        var category = _mapper.Map<CategoryCreateDTO, Category>(newCategory);
        var result = await _categoryRepo.CreateOneAsync(category);
        return _mapper.Map<Category, CategoryReadDTO>(result);
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        var categoryToDelete = await _categoryRepo.GetOneByIdAsync(id);
        if (categoryToDelete != null)
        {
            return await _categoryRepo.DeleteOneAsync(categoryToDelete);            
        }
        throw CustomException.NotFoundException("Category not found");
    }

    public async Task<IEnumerable<CategoryReadDTO>> GetAllCategoriesAsync(GetAllParams options)
    {
        var result = await _categoryRepo.GetAllAsync(options);
        return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryReadDTO>>(result);
    }

    public async Task<CategoryReadDTO> GetCategoryByIdAsync(Guid id)
    {
        var result = await _categoryRepo.GetOneByIdAsync(id);
        if (result != null)
        {
            return _mapper.Map<Category, CategoryReadDTO>(result);
        }
        else
        {
            throw CustomException.NotFoundException("Category not found");
        }
    }

    public async Task<CategoryReadDTO> UpdateCategoryAsync(Guid id, CategoryUpdateDTO updates)
    {
        var categoryToUpdate = await _categoryRepo.GetOneByIdAsync(id);
        if (categoryToUpdate != null)
        {
            var updatedCategory = updates.Merge(categoryToUpdate);
            var updated = await _categoryRepo.UpdateOneAsync(updatedCategory);
            return _mapper.Map<Category, CategoryReadDTO>(updated);
        }
        else
        {
            throw CustomException.NotFoundException("Category not found");
        }
    }
}