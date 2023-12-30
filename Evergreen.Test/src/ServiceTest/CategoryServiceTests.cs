using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Evergreen.Service.src.Service;
using Evergreen.Service.src.Shared;
using Moq;

namespace Evergreen.Test.src;

public class CategoryServiceTests
{
    private static IMapper _mapper;

    public CategoryServiceTests()
    {
        if (_mapper == null)
        {
            var mappingConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new MapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper; 
        }
    }

    [Fact]
    public async void GetAllAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<ICategoryRepository>();
        var mapper = new Mock<IMapper>();
        var categoryService = new CategoryService(repo.Object, _mapper);

        await categoryService.GetAllCategoriesAsync();

        repo.Verify(repo => repo.GetAllParameterlessAsync(), Times.Once);
    }

    [Theory]
    [ClassData(typeof(GetAllCategoriesData))]
    public async void GetAllAsync_ShouldReturnValidResponse(IEnumerable<Category> repoResponse, IEnumerable<CategoryReadDTO> expected)
    {
        var repo = new Mock<ICategoryRepository>();
        repo.Setup(repo => repo.GetAllParameterlessAsync()).ReturnsAsync(repoResponse);
        var categoryService = new CategoryService(repo.Object, _mapper);
        
        var response = await categoryService.GetAllCategoriesAsync();

        Assert.Equivalent(expected, response);
    }

    public class GetAllCategoriesData : TheoryData<IEnumerable<Category>, IEnumerable<CategoryReadDTO>>
    {
        public GetAllCategoriesData()
        {
            Category category1 = new Category(){Name = "Air plants", ImageUrl = "https://picsum.photos/200"};
            Category category2 = new Category(){Name = "Succulents", ImageUrl = "https://picsum.photos/200"};
            Category category3 = new Category(){Name = "Cacti", ImageUrl = "https://picsum.photos/200"};
            IEnumerable<Category> categories = new List<Category>(){category1, category2, category3};
            Add(categories, _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryReadDTO>>(categories));
        }
    }

    [Fact]
    public async void GetCategoryById_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<ICategoryRepository>();
        Category category1 = new Category(){Name = "Air plants", ImageUrl = "https://picsum.photos/200"};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(category1);
        var mapper = new Mock<IMapper>();
        var categoryService = new CategoryService(repo.Object, _mapper);

        await categoryService.GetCategoryByIdAsync(It.IsAny<Guid>());

        repo.Verify(repo => repo.GetOneByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(GetOneCategoryData))]
    public async void GetCategoryById_ShouldReturnValidResponse(Category? repoResponse, CategoryReadDTO? expected, Type? exceptionType)
    {
        var repo = new Mock<ICategoryRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(repoResponse));
        var categoryService = new CategoryService(repo.Object, _mapper);

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => categoryService.GetCategoryByIdAsync(It.IsAny<Guid>()));
        }
        else
        {
            var response = await categoryService.GetCategoryByIdAsync(It.IsAny<Guid>());

            Assert.Equivalent(expected, response);
        }
    
    }

    public class GetOneCategoryData : TheoryData<Category?, CategoryReadDTO?, Type?>
    {
        public GetOneCategoryData()
        {
            Category category1 = new Category(){Name = "Air plants", ImageUrl = "https://picsum.photos/200"};
            CategoryReadDTO category1Read = _mapper.Map<Category, CategoryReadDTO>(category1);
            Add(category1, category1Read, null);
            Add(null, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void CreateOneAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<ICategoryRepository>();
        var mapper = new Mock<IMapper>();
        var categoryService = new CategoryService(repo.Object, _mapper);
        CategoryCreateDTO dto = new CategoryCreateDTO(){ Name = "Air plants", ImageUrl = "https://picsum.photos/200"};

        await categoryService.CreateCategoryAsync(dto);

        repo.Verify(repo => repo.CreateOneAsync(It.IsAny<Category>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(CreateCategoryData))]
    public async void CreateOneAsync_ShouldReturnValidResponse(Category repoResponse, CategoryReadDTO expected, Type? exceptionType)
    {
        var repo = new Mock<ICategoryRepository>();
        repo.Setup(repo => repo.CreateOneAsync(It.IsAny<Category>())).ReturnsAsync(repoResponse);
        var categoryService = new CategoryService(repo.Object, _mapper);
        CategoryCreateDTO dto = new CategoryCreateDTO(){ Name = "Air plants", ImageUrl = "https://picsum.photos/200"};

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => categoryService.CreateCategoryAsync(dto));
        }
        else
        {
            var response = await categoryService.CreateCategoryAsync(dto);

            Assert.Equivalent(expected, response);
        }
    }

    public class CreateCategoryData : TheoryData<Category?, CategoryReadDTO?, Type?>
    {
        public CreateCategoryData()
        {
            Category category1 = new Category(){Name = "Air plants", ImageUrl = "https://picsum.photos/200"};
            CategoryReadDTO category1Read = _mapper.Map<Category, CategoryReadDTO>(category1);
            Add(category1, category1Read, null);
        }
    }

    [Fact]
    public async void DeleteCategoryAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<ICategoryRepository>();
        Category category1 = new Category(){Name = "Air plants", ImageUrl = "https://picsum.photos/200"};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(category1);
        var mapper = new Mock<IMapper>();
        var categoryService = new CategoryService(repo.Object, _mapper);

        await categoryService.DeleteCategoryAsync(It.IsAny<Guid>());

        repo.Verify(repo => repo.DeleteOneAsync(It.IsAny<Category>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(DeleteCategoryData))]
    public async void DeleteCategoryAsync_ShouldReturnValidResponse(Category? foundResponse, bool repoResponse, bool? expected, Type? exceptionType)
    {
        var repo = new Mock<ICategoryRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(foundResponse);
        repo.Setup(repo => repo.DeleteOneAsync(It.IsAny<Category>())).ReturnsAsync(repoResponse);
        var categoryService = new CategoryService(repo.Object, _mapper);

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => categoryService.DeleteCategoryAsync(It.IsAny<Guid>()));
        }
        else
        {
            var response = await categoryService.DeleteCategoryAsync(It.IsAny<Guid>());

            Assert.Equivalent(expected, response);
        }
    
    }

    public class DeleteCategoryData : TheoryData<Category?, bool, bool?, Type?>
    {
        public DeleteCategoryData()
        {
            Category category1 = new Category(){Name = "Air plants", ImageUrl = "https://picsum.photos/200"};
            Add(category1, true, true, null);
            Add(null, false, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void UpdateCategoryAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<ICategoryRepository>();
        Category category1 = new Category(){Name = "Air plants", ImageUrl = "https://picsum.photos/200"};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(category1);
        var mapper = new Mock<IMapper>();
        var categoryService = new CategoryService(repo.Object, _mapper);
        CategoryUpdateDTO updates = new CategoryUpdateDTO(){Name = "Tillandsia", ImageUrl = "https://picsum.photos/300"};

        await categoryService.UpdateCategoryAsync(It.IsAny<Guid>(), updates);

        repo.Verify(repo => repo.UpdateOneAsync(It.IsAny<Category>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(UpdateCategoryData))]
    public async void UpdateCategoryAsync_ShouldReturnValidResponse(CategoryUpdateDTO updates, Category? foundResponse, Category repoResponse, CategoryReadDTO? expected, Type? exceptionType)
    {
        var repo = new Mock<ICategoryRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(foundResponse);
        repo.Setup(repo => repo.UpdateOneAsync(It.IsAny<Category>())).ReturnsAsync(repoResponse);
        var categoryService = new CategoryService(repo.Object, _mapper);

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => categoryService.UpdateCategoryAsync(It.IsAny<Guid>(), updates));
        }
        else
        {
            var response = await categoryService.UpdateCategoryAsync(It.IsAny<Guid>(), updates);

            Assert.Equivalent(expected, response);
        }
    }

    public class UpdateCategoryData : TheoryData< CategoryUpdateDTO, Category?, Category?, CategoryReadDTO?, Type?>
    {
        public UpdateCategoryData()
        {
            CategoryUpdateDTO updates1 = new CategoryUpdateDTO(){Name = "Tillandsia", ImageUrl = "https://picsum.photos/300"};
            CategoryUpdateDTO updates2 = new CategoryUpdateDTO(){Name = "Tillandsia"};
            Category category1 = new Category(){Name = "Air plants", ImageUrl = "https://picsum.photos/200"};
            CategoryReadDTO category1Read = _mapper.Map<Category, CategoryReadDTO>(category1);
            Add(updates1, new Category(){}, category1, category1Read, null);
            Add(updates2, new Category(){}, category1, category1Read, null);
            Add(updates1, null, category1, null, typeof(CustomException));
        }
    }

}