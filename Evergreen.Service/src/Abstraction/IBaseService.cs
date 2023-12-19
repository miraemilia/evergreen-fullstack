using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;

namespace Evergreen.Service.src.Abstraction;

public interface IBaseService<T, TReadDTO, TCreateDTO, TUpdateDTO> where T : BaseEntity
{
    Task<IEnumerable<TReadDTO>> GetAllAsync(GetAllParams options);
    Task<TReadDTO?> GetOneByIdAsync(Guid id);
    Task<TReadDTO> CreateOneAsync(TCreateDTO newItem);
    Task<bool> DeleteOneAsync(Guid id);
    Task<TReadDTO> UpdateOneAsync(Guid id, TUpdateDTO updates);
}