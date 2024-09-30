
using Application.Common.Models.BreedModels;
using Application.Common.Models.CategoryModels;
using Application.Common.Models.FishAwardModels;
using Application.Common.Models.FishModels;
using Application.Common.Models.ProductModels;
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
            .ForMember(dest => dest.Fish, opt => opt.Ignore());

        CreateMap<FishProductCreateModel, Product>();
            

        // Tank
        CreateMap<TankCreateModel, Tank>();

        // Fish
        CreateMap<FishCreateRequestModel, Fish>()
            .ForMember(dest => dest.Sex, opt => opt.Ignore())
            .ForMember(dest => dest.DateOfBirth, opt => opt.Ignore());
        // Category
        CreateMap<CategoryCreateModel, Category>();
        // CreateMap<Category>()
        
        // Breed
        CreateMap<Breed, BreedResponseModel>();
        CreateMap<BreedCreateRequestModel, Breed>();

        //FishAward
        CreateMap<FishAwardCreateRequestModel, FishAward>();
    }
}