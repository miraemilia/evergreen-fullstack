using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Evergreen.Service.src.Shared;

namespace Evergreen.Service.src.Service;

public class BaseService<T, TRepo, TPageableReadDTO, TReadDTO, TCreateDTO, TUpdateDTO> : IBaseService<T, TRepo, TPageableReadDTO, TReadDTO, TCreateDTO, TUpdateDTO> where T : BaseEntity where TRepo : IBaseRepository<T> where TPageableReadDTO : BasePageableReadDTO<TReadDTO>
{
    protected TRepo _repo;
    protected IMapper _mapper;

    public BaseService(TRepo repo, IMapper mapper)
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

    public virtual async Task<BasePageableReadDTO<TReadDTO>> GetAllAsync(GetAllParams options)
    {
        var result = await _repo.GetAllAsync(options);
        var total = await _repo.GetCountAsync(options);
        var foundItems = _mapper.Map<IEnumerable<T>, IEnumerable<TReadDTO>>(result);
        int pages = (total + options.Limit -1)/options.Limit;
        var response = new BasePageableReadDTO<TReadDTO>(){Items = foundItems, TotalItems = total, Pages = pages};
        return response;
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