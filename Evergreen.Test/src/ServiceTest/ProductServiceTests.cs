using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.DTO;
using Evergreen.Service.src.Service;
using Evergreen.Service.src.Shared;
using Moq;

namespace Evergreen.Test.src;

public class ProductServiceTests
{
    private static IMapper _mapper;

    public ProductServiceTests()
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
    public async void GetAllProductsAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IProductRepository>();
        var categoryRepo = new Mock<ICategoryRepository>();
        var imageRepo = new Mock<IImageRepository>();
        var mapper = new Mock<IMapper>();
        var productService = new ProductService(repo.Object, _mapper, categoryRepo.Object, imageRepo.Object);
        GetAllParams options = new GetAllParams(){Limit = 10, Offset = 0};

        await productService.GetAllProductsAsync(options);

        repo.Verify(repo => repo.GetAllAsync(options), Times.Once);
    }

    [Fact]
    public async void GetProductById_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IProductRepository>();
        var categoryRepo = new Mock<ICategoryRepository>();
        var imageRepo = new Mock<IImageRepository>();
        Product product1 = new Product(){};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product1);
        var mapper = new Mock<IMapper>();
        var productService = new ProductService(repo.Object, _mapper, categoryRepo.Object, imageRepo.Object);

        await productService.GetProductByIdAsync(It.IsAny<Guid>());

        repo.Verify(repo => repo.GetOneByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(GetOneProductData))]
    public async void GetProductById_ShouldReturnValidResponse(Product? repoResponse, ProductReadDTO? expected, Type? exceptionType)
    {
        var repo = new Mock<IProductRepository>();
        var categoryRepo = new Mock<ICategoryRepository>();
        var imageRepo = new Mock<IImageRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(repoResponse));
        var productService = new ProductService(repo.Object, _mapper, categoryRepo.Object, imageRepo.Object);

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => productService.GetProductByIdAsync(It.IsAny<Guid>()));
        }
        else
        {
            var response = await productService.GetProductByIdAsync(It.IsAny<Guid>());

            Assert.Equivalent(expected, response);
        }
    
    }

    public class GetOneProductData : TheoryData<Product?, ProductReadDTO?, Type?>
    {
        public GetOneProductData()
        {
            Product product1 = new Product(){};
            ProductReadDTO product1Read = _mapper.Map<Product, ProductReadDTO>(product1);
            Add(product1, product1Read, null);
            Add(null, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void CreateOneAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IProductRepository>();
        var categoryRepo = new Mock<ICategoryRepository>();
        var imageRepo = new Mock<IImageRepository>();
        categoryRepo.Setup(categoryRepo => categoryRepo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Category(){});
        var mapper = new Mock<IMapper>();
        var productService = new ProductService(repo.Object, _mapper, categoryRepo.Object, imageRepo.Object);
        ProductCreateDTO dto = new ProductCreateDTO(){};

        await productService.CreateProductAsync(dto);

        repo.Verify(repo => repo.CreateOneAsync(It.IsAny<Product>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(CreateProductData))]
    public async void CreateOneAsync_ShouldReturnValidResponse(Category? categoryResponse, Product repoResponse, ProductReadDTO expected, Type? exceptionType)
    {
        var repo = new Mock<IProductRepository>();
        repo.Setup(repo => repo.CreateOneAsync(It.IsAny<Product>())).ReturnsAsync(repoResponse);
        var categoryRepo = new Mock<ICategoryRepository>();
        var imageRepo = new Mock<IImageRepository>();
        categoryRepo.Setup(categoryRepo => categoryRepo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(categoryResponse);
        var productService = new ProductService(repo.Object, _mapper, categoryRepo.Object, imageRepo.Object);
        ProductCreateDTO dto = new ProductCreateDTO(){};

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => productService.CreateProductAsync(dto));
        }
        else
        {
            var response = await productService.CreateProductAsync(dto);

            Assert.Equivalent(expected, response);
        }
    }

    public class CreateProductData : TheoryData<Category?, Product?, ProductReadDTO?, Type?>
    {
        public CreateProductData()
        {
            Product product1 = new Product(){};
            Category category1 = new Category(){};
            ProductReadDTO product1Read = new ProductReadDTO(){};
            product1Read.ProductImages = new List<ProductImageReadDTO>(){};
            Add(category1, product1, product1Read, null);
            Add(null, null, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void DeleteProductAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IProductRepository>();
        var categoryRepo = new Mock<ICategoryRepository>();
        var imageRepo = new Mock<IImageRepository>();
        Product product1 = new Product(){};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product1);
        var mapper = new Mock<IMapper>();
        var productService = new ProductService(repo.Object, _mapper, categoryRepo.Object, imageRepo.Object);

        await productService.DeleteProductAsync(It.IsAny<Guid>());

        repo.Verify(repo => repo.DeleteOneAsync(It.IsAny<Product>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(DeleteProductData))]
    public async void DeleteProductAsync_ShouldReturnValidResponse(Product? foundResponse, bool repoResponse, bool? expected, Type? exceptionType)
    {
        var repo = new Mock<IProductRepository>();
        var categoryRepo = new Mock<ICategoryRepository>();
        var imageRepo = new Mock<IImageRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(foundResponse);
        repo.Setup(repo => repo.DeleteOneAsync(It.IsAny<Product>())).ReturnsAsync(repoResponse);
        var productService = new ProductService(repo.Object, _mapper, categoryRepo.Object, imageRepo.Object);

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => productService.DeleteProductAsync(It.IsAny<Guid>()));
        }
        else
        {
            var response = await productService.DeleteProductAsync(It.IsAny<Guid>());

            Assert.Equivalent(expected, response);
        }
    
    }

    public class DeleteProductData : TheoryData<Product?, bool, bool?, Type?>
    {
        public DeleteProductData()
        {
            Product product1 = new Product(){};
            Add(product1, true, true, null);
            Add(null, false, null, typeof(CustomException));
        }
    }

/* 
 */

}