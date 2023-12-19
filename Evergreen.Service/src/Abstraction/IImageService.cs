using Evergreen.Core.src.Entity;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Abstraction;
public interface IImageService : IBaseService<Image, ImageReadDTO, ImageCreateDTO, ImageUpdateDTO>
{}