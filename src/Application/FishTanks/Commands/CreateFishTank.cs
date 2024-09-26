// using FishEcomerce.Application.Common.Interfaces;
// using FishEcomerce.Infrastructure.Context;
//
// namespace Microsoft.Extensions.DependencyInjection.FishTanks.Commands;
//
// public record CreateFishTankCommand : IRequest<int>
// {
//     public string? Name { get; set; }
//
//     public string? Description { get; set; }
//
//     public string? InformationDetail { get; set; }
//
//     public string? Size { get; set; }
//
//     public string? SizeInformation { get; set; }
//
//     public string? GlassType { get; set; }
//     
// }
//
// public class CreateFishTankCommandHandler : IRequestHandler<CreateFishTankCommand, int>
// {
//     private readonly KingFishContext _context;
//
//     public CreateFishTankCommandHandler(KingFishContext context)
//     {
//         _context = context;
//     }
//
//     public async Task<int> Handle(CreateFishTankCommand request, CancellationToken cancellationToken)
//     {
//         
//     }
// }
