//using System.Net;
//using Application.Common.Models;
//using Application.Common.UoW;

//namespace Application.FishAwards.Commands.DeleteFishAward;

//public record DeleteFishAwardCommand : IRequest<ResponseModel>
//{
//    public Guid Id { get; set; }
//}

//public class DeleteFishAwardCommandHandler : IRequestHandler<DeleteFishAwardCommand, ResponseModel>
//{
//    private readonly IUnitOfWork _unitOfWork;

//    public DeleteFishAwardCommandHandler(IUnitOfWork unitOfWork)
//    {
//        _unitOfWork = unitOfWork;
//    }
//    public async Task<ResponseModel> Handle(DeleteFishAwardCommand request, CancellationToken cancellationToken)
//    {
//        var fishAward = await _unitOfWork.FishAwardRepository.GetByIdAsync(request.Id);
//        if (fishAward is null)
//        {
//            return new ResponseModel(HttpStatusCode.BadRequest, "Fish award not found.");
//        }
        
//        await _unitOfWork.BeginTransactionAsync();
//        try
//        {
//            _unitOfWork.FishAwardRepository.Update(fishAward);
//            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
//            if (result > 0)
//            {
//                await _unitOfWork.CommitTransactionAsync();
//                return new ResponseModel(HttpStatusCode.OK, "Product has been deleted.");
//            }
//            await _unitOfWork.RollbackTransactionAsync();
//            return new ResponseModel(HttpStatusCode.BadRequest, "Product has NOT been deleted.");
//        }
//        catch (Exception e)
//        {
//            await _unitOfWork.RollbackTransactionAsync();
//            return new ResponseModel(HttpStatusCode.BadRequest, e.Message);
//        }
        
//    }
//}