using Domain.Entites;
using Infrastructure.Context;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
{
    public FeedbackRepository(KingFishDbContext context) : base(context)
    {
    }
    
    
}