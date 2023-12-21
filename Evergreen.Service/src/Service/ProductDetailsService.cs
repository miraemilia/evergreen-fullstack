using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Evergreen.Service.src.Shared;

namespace Evergreen.Service.src.Service;

public class ProductDetailsService : BaseService<ProductDetails, IProductDetailsRepository, ProductDetailsPageableReadDTO, ProductDetailsReadDTO, ProductDetailsCreateDTO, ProductDetailsUpdateDTO>, IProductDetailsService
{
    public ProductDetailsService(IProductDetailsRepository repo, IMapper mapper) : base(repo, mapper)
    {}
    public override async Task<ProductDetailsReadDTO> UpdateOneAsync(Guid id, ProductDetailsUpdateDTO updates)
    {
        ProductDetails? details = await _repo.GetOneByProductIdAsync(id);
        if (details is null)
        {
            throw CustomException.NotFoundException("Product details not found");
        }
        var updatedDetails = _mapper.Map(updates, details);
        var response = await _repo.UpdateOneAsync(updatedDetails);
        return _mapper.Map<ProductDetails, ProductDetailsReadDTO>(response);
    }
}