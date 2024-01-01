using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.WebAPI.src.Database;

namespace Evergreen.WebAPI.src.Repository;

public class ImageRepository : BaseRepository<Image>, IImageRepository
{
    public ImageRepository(DatabaseContext database) : base(database)
    {}
}