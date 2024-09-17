using FishEcomerce.Domain.Entities;

namespace FishEcomerce.Application.Common.Interfaces;

public interface IKingFishDbContext
{
    DbSet<Blog> Blogs { get; }

    DbSet<Breed> Breeds { get; }

    DbSet<Comment> Comments { get; }

    DbSet<Customer> Customers { get; }

    DbSet<Feedback> Feedbacks { get; }

    DbSet<FishAward> FishAwards { get; }

    DbSet<FishProduct> FishProducts { get; }

    DbSet<FishTank> FishTanks { get; }

    DbSet<FishTankCategory> FishTankCategories { get; }

    DbSet<Image> Images { get; }

    DbSet<Order> Orders { get; }

    DbSet<OrderDetail> OrderDetails { get; }

    DbSet<Product> Products { get; }

    DbSet<Supplier> Suppliers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
