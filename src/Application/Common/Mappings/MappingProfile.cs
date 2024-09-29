
using Application.Common.Models.ProductModels;
using Application.Common.Models.TankModels;
using Domain.Entites;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product
        CreateMap<ProductCreateModel, Product>()
            .ForMember(dest => dest.Tank, opt => opt.Ignore())
            .ForMember(dest => dest.Fish, opt => opt.Ignore());
        
        // Tank
        CreateMap<TankCreateModel, Tank>();
    }
}