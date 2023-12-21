using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Abstraction;

public interface IBaseService<T, TRepo, TPageableReadDTO, TReadDTO, TCreateDTO, TUpdateDTO> where T : BaseEntity where TRepo : IBaseRepository<T> where TPageableReadDTO : BasePageableReadDTO<TReadDTO>
{
    Task<BasePageableReadDTO<TReadDTO>> GetAllAsync(GetAllParams options);
    Task<TReadDTO?> GetOneByIdAsync(Guid id);
    Task<TReadDTO> CreateOneAsync(TCreateDTO newItem);
    Task<bool> DeleteOneAsync(Guid id);
    Task<TReadDTO> UpdateOneAsync(Guid id, TUpdateDTO updates);
}