using AutoMapper;
using Evergreen.Core.src.Entity;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Shared;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserReadDTO>();
        CreateMap<UserCreateDTO, User>();
        CreateMap<UserUpdateDTO, User>()
            .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));
        CreateMap<UserWithRoleCreateDTO, User>();

        CreateMap<Category, CategoryReadDTO>();
        CreateMap<CategoryCreateDTO, Category>();
        CreateMap<CategoryUpdateDTO, Category>()
            .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        CreateMap<ProductDetailsCreateDTO, ProductDetails>();
        CreateMap<ProductDetails, ProductDetailsReadDTO>();
        CreateMap<ProductDetailsUpdateDTO, ProductDetails>()
            .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));

        CreateMap<Image, ImageReadDTO>();
        CreateMap<ImageCreateDTO, Image>();
        CreateMap<ImageUpdateDTO, Image>()
            .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));
        
        CreateMap<Product, ProductReadDTO>()
            .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(s => s.ProductImages))
            .ForMember(dest => dest.ProductDetails, opt => opt.MapFrom(s => s.ProductDetails));
        CreateMap<ProductCreateDTO, Product>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.ProductDetails, opt => opt.MapFrom(s => s.ProductDetails))
            .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(s => s.ProductImages));
        CreateMap<ProductUpdateDTO, Product>()
            .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));
        CreateMap<List<Image>, Product>()
            .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(s => s));

        CreateMap<OrderProduct, OrderProductReadDTO>();
        CreateMap<OrderProductCreateDTO, OrderProduct>();

        CreateMap<Order, OrderReadDTO>()
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(s => s.OrderDetails));
        CreateMap<OrderCreateDTO, Order>()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(s => s.OrderDetails));
        CreateMap<OrderUpdateDTO, Order>()
            .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null));
    }
}