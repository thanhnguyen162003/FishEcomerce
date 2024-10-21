using Application.Common.Models.BlogModel;
using Application.Common.Models.BreedModels;
using Application.Common.Models.CategoryModels;
using Application.Common.Models.CustomerModels;
using Application.Common.Models.FeedbackModels;
using Application.Common.Models.FishAwardModels;
using Application.Common.Models.FishModels;
using Application.Common.Models.ImageModels;
using Application.Common.Models.OrderDetailModels;
using Application.Common.Models.OrderModels;
using Application.Common.Models.ProductModels;
using Application.Common.Models.StaffModels;
using Application.Common.Models.TankModels;
using Domain.Entites;


namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product
        CreateMap<TankProductCreateModel, Product>()
            .ForMember(dest => dest.Tank, opt => opt.Ignore())
            .ForMember(dest => dest.Fish, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore());

        CreateMap<FishProductCreateModel, Product>();
        CreateMap<Product, ProductResponseModel>()
            .ForMember(x => x.Fish, opt => opt.MapFrom(src => src.Fish)).ReverseMap()
            .ForMember(x => x.Tank, opt => opt.MapFrom(src => src.Tank)).ReverseMap()
            .ForMember(x => x.Feedbacks, opt => opt.MapFrom(src => src.Feedbacks)).ReverseMap(); 
        // Tank
        CreateMap<TankCreateModel, Tank>();
        CreateMap<Tank, TankResponseModel>();

        // Fish
        CreateMap<FishCreateRequestModel, Fish>()
            .ForMember(dest => dest.Sex, opt => opt.Ignore())
            .ForMember(dest => dest.DateOfBirth, opt => opt.Ignore());
        CreateMap<Fish, FishResponseModel>().ReverseMap();
        // Category
        CreateMap<CategoryCreateModel, Category>();
        CreateMap<Category, CategoryResponseModel>();
        
        // Breed
        CreateMap<Breed, BreedResponseModel>().ReverseMap();
        CreateMap<BreedCreateRequestModel, Breed>();

        
        // Feedback
        CreateMap<FeedbackCreateModel, Feedback>();
        CreateMap<Feedback, FeedbackResponseModel>();

        //FishAward
        CreateMap<FishAwardCreateRequestModel, FishAward>();
        CreateMap<FishAward, FishAwardResponseModel>();
        
        // Image
        CreateMap<Image, ImageResponseModel>();

        // Blog
        CreateMap<BlogCreateRequestModel, Domain.Entites.Blog>().ReverseMap();
        CreateMap<BlogUpdateRequestModel, Domain.Entites.Blog>().ReverseMap();
        CreateMap<Domain.Entites.Blog, BlogResponseModel>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Staff!.FullName))
            .ReverseMap();
        
        // Order & OrderDetail
        CreateMap<OrderCreateModel, Domain.Entites.Order>()
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()));
        CreateMap<OrderDetailCreateModel, OrderDetail>();
        
        CreateMap<Domain.Entites.Order, OrderResponseModel>();
        CreateMap<OrderDetail, OrderDetailResponseModel>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.Name));
        
        // Customer
        CreateMap<Customer, CustomerResponseModel>();
        
        // Staff
        CreateMap<StaffCreateModel, Staff>();
    }
}