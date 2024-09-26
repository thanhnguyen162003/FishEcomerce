using FishEcomerce.Domain.Entities;
using Microsoft.Extensions.DependencyInjection.Products.Commands;

namespace FishEcomerce.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProductCommand, Product>();
    }
}
