using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Service;

public class ImageService : BaseService<Image, ImageReadDTO, ImageCreateDTO, ImageUpdateDTO>, IImageService
{
    public ImageService(IBaseRepository<Image> repo, IMapper mapper) : base(repo, mapper)
    {}
}