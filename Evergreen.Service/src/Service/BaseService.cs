using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.Shared;

namespace Evergreen.Service.src.Service;

public class BaseService<T, TReadDTO, TCreateDTO, TUpdateDTO> : IBaseService<T, TReadDTO, TCreateDTO, TUpdateDTO> where T : BaseEntity
{
    protected IBaseRepository<T> _repo;
    protected IMapper _mapper;

    public BaseService(IBaseRepository<T> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public virtual async Task<TReadDTO> CreateOneAsync(TCreateDTO newItem)
    {
        var item = _mapper.Map<TCreateDTO, T>(newItem);
        var result = await _repo.CreateOneAsync(item);
        return _mapper.Map<T, TReadDTO>(result);    }

    public virtual async Task<bool> DeleteOneAsync(Guid id)
    {
        var itemToDelete = await _repo.GetOneByIdAsync(id);
        if (itemToDelete != null)
        {
            return await _repo.DeleteOneAsync(itemToDelete);            
        }
        throw CustomException.NotFoundException("Could not delete: item not found");
    }

    public virtual async Task<IEnumerable<TReadDTO>> GetAllAsync(GetAllParams options)
    {
        var result = await _repo.GetAllAsync(options);
        return _mapper.Map<IEnumerable<T>, IEnumerable<TReadDTO>>(result);
    }

    public virtual async Task<TReadDTO?> GetOneByIdAsync(Guid id)
    {
        var result = await _repo.GetOneByIdAsync(id);
        if (result != null)
        {
            return _mapper.Map<T, TReadDTO>(result);
        }
        else
        {
            throw CustomException.NotFoundException("Item not found");
        }
    }

    public virtual async Task<TReadDTO> UpdateOneAsync(Guid id, TUpdateDTO updates)
    {
        var itemToUpdate = await _repo.GetOneByIdAsync(id);
        if (itemToUpdate != null)
        {
            var updatedItem = _mapper.Map(updates, itemToUpdate);
            var updated = await _repo.UpdateOneAsync(updatedItem);
            return _mapper.Map<T, TReadDTO>(updated);
        }
        else
        {
            throw CustomException.NotFoundException("Could not update: item not found");
        }
    }
}