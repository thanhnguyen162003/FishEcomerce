using Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class KingFishDbContext : DbContext
{
    public KingFishDbContext() { }
    
    public KingFishDbContext(DbContextOptions<KingFishDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=20.205.21.96;Port=5432;Database=kingfish;Username=root;Password=Admin123456789@");
    
    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Breed> Breeds { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }
    
    public virtual DbSet<Fish> Fishes { get; set; }

    public virtual DbSet<FishAward> FishAwards { get; set; }

    public virtual DbSet<Image> Images { get; set; }
    
    public virtual DbSet<Order> Orders { get; set; }
    
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    
    public virtual DbSet<Product> Products { get; set; }
    
    public virtual DbSet<Supplier> Suppliers { get; set; }
    
    public virtual DbSet<Tank> Tanks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Blog_pkey");

            entity.ToTable("Blog");

            entity.HasIndex(e => e.SupplierId, "IX_Blog_supplierId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Content)
                .HasColumnType("character varying")
                .HasColumnName("content");
            entity.Property(e => e.ContentHtml)
                .HasColumnType("character varying")
                .HasColumnName("contentHtml");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deletedAt");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.SupplierId).HasColumnName("supplierId");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("Blog_supplierId_fkey");
        });
        
        modelBuilder.Entity<Breed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Breed_pkey");

            entity.ToTable("Breed");
            
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deletedAt");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedAt");
        });
        
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Category_pkey");

            entity.ToTable("Category");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deletedAt");
            entity.Property(e => e.TankType)
                .HasMaxLength(255)
                .HasColumnName("TankType");
            entity.Property(e => e.Level)
                .HasMaxLength(255)
                .HasColumnName("level");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedAt");
        });
        
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Comment_pkey");

            entity.ToTable("Comment");

            entity.HasIndex(e => e.BlogId, "IX_Comment_blogId");

            entity.HasIndex(e => e.CustomerId, "IX_Comment_customerId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BlogId).HasColumnName("blogId");
            entity.Property(e => e.Content)
                .HasColumnType("character varying")
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deletedAt");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("Comment_blogId_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("Comment_customerId_fkey");
        });
        
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Customer_pkey");

            entity.ToTable("Customer");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasColumnType("character varying")
                .HasColumnName("address");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deletedAt");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.LoyaltyPoints).HasColumnName("loyaltyPoints");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.RegistrationDate).HasColumnName("registrationDate");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedAt");
        });
        
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Feedback_pkey");

            entity.ToTable("Feedback");

            entity.HasIndex(e => e.ProductId, "IX_Feedback_productId");

            entity.HasIndex(e => e.UserId, "IX_Feedback_userId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Content)
                .HasColumnType("character varying")
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deletedAt");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Product).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("Feedback_productId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Feedback_userId_fkey");
        });
        
        modelBuilder.Entity<Fish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Fish_pkey");

            entity.ToTable("Fish");

            entity.HasIndex(e => e.ProductId, "IX_Fish_productId");
            entity.HasIndex(e => e.BreedId, "IX_Fish_breedId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Age)
                .HasMaxLength(255)
                .HasColumnName("age");
            entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");
            entity.Property(e => e.FoodAmount)
                .HasMaxLength(255)
                .HasColumnName("foodAmount");
            entity.Property(e => e.Health)
                .HasMaxLength(255)
                .HasColumnName("health");
            entity.Property(e => e.Origin)
                .HasMaxLength(255)
                .HasColumnName("origin");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e=> e.BreedId).HasColumnName("breedId");
            entity.Property(e => e.Sex)
                .HasMaxLength(50)
                .HasColumnName("sex");
            entity.Property(e => e.Size)
                .HasMaxLength(255)
                .HasColumnName("size");
            entity.Property(e => e.Weight)
                .HasPrecision(18, 2)
                .HasColumnName("weight");

            entity.HasOne(d => d.Breed).WithMany(p => p.Fishes)
                .HasForeignKey(d => d.BreedId)
                .HasConstraintName("Fish_breedId_fkey");;
            
            entity.HasOne(d => d.Product).WithOne(p => p.Fish)
                .HasForeignKey<Fish>(d => d.ProductId)
                .HasConstraintName("Fish_productId_fkey");
        });
        
        modelBuilder.Entity<FishAward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FishAward_pkey");

            entity.ToTable("FishAward");

            entity.HasIndex(e => e.FishId, "IX_FishAward_fishId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.FishId).HasColumnName("fishId");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Image)
                .HasColumnType("character varying")
                .HasColumnName("image");
            entity.Property(e => e.AwardDate).HasColumnName("awardDate");
            
            entity.HasOne(d => d.Fish).WithMany(p => p.Awards)
                .HasForeignKey(d => d.FishId)
                .HasConstraintName("FishAward_fishId_fkey");
        });
        
        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Image_pkey");

            entity.ToTable("Image");

            entity.HasIndex(e => e.BlogId, "IX_Image_blogId");

            entity.HasIndex(e => e.ProductId, "IX_Image_productId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BlogId).HasColumnName("blogId");
            entity.Property(e => e.PublicId)
                .HasColumnType("character varying")
                .HasColumnName("publicId");
            entity.Property(e => e.Link)
                .HasColumnType("character varying")
                .HasColumnName("cloudLink");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deletedAt");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Blog).WithMany(p => p.Images)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("Image_blogId_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.Images)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("Image_productId_fkey");
        });
        
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Orders_pkey");

            entity.HasIndex(e => e.CustomerId, "IX_Orders_customerId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.CustomerId).HasColumnName("customerId");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deletedAt");
            entity.Property(e => e.OrderDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("orderDate");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
            entity.Property(e => e.PaymentMethod)
                .HasColumnType("character varying")
                .HasColumnName("paymentMethod");
            entity.Property(e => e.OrderCode)
                .HasColumnName("orderCode")
                .HasColumnType("bigint");
            entity.Property(e => e.IsPaid)
                .HasColumnName("isPaid");
            entity.Property(e => e.ShipAddress)
                .HasColumnType("character varying")
                .HasColumnName("shipAddress");
            entity.Property(e => e.ShippedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("shippedDate");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(18, 2)
                .HasColumnName("totalPrice");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("Orders_customerId_fkey");
        });
        
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("OrderDetail_pkey");

            entity.ToTable("OrderDetail");

            entity.HasIndex(e => e.OrderId, "IX_OrderDetail_orderId");

            entity.HasIndex(e => e.ProductId, "IX_OrderDetail_productId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(18, 2)
                .HasColumnName("unitPrice");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(18, 2)
                .HasColumnName("totalPrice");
            entity.Property(e => e.Discount)
                .HasPrecision(18, 2)
                .HasColumnName("discount");
            
            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("OrderDetail_orderId_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("OrderDetail_productId_fkey");
        });
        
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Product_pkey");

            entity.ToTable("Product");

            entity.HasIndex(e => e.SupplierId, "IX_Product_supplierId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("Type");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deletedAt");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.DescriptionDetail)
                .HasColumnType("character varying")
                .HasColumnName("descriptionDetail");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(18, 2)
                .HasColumnName("price");
            entity.Property(e => e.OriginalPrice)
                .HasPrecision(18, 2)
                .HasColumnName("orginalPrice");
            entity.Property(e => e.Slug)
                .HasColumnType("character varying")
                .HasColumnName("slug");
            entity.Property(e => e.Sold).HasColumnName("sold");
            entity.Property(e => e.StockQuantity).HasColumnName("stockQuantity");
            entity.Property(e => e.SupplierId).HasColumnName("supplierId");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("Product_supplierId_fkey");
        });
        
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Supplier_pkey");

            entity.ToTable("Supplier");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AddressStore)
                .HasColumnType("character varying")
                .HasColumnName("addressStore");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .HasColumnName("companyName");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deletedAt");
            entity.Property(e => e.Facebook)
                .HasMaxLength(255)
                .HasColumnName("facebook");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });
        
        modelBuilder.Entity<Tank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Tank_pkey");

            entity.ToTable("Tank");

            entity.HasIndex(e => e.ProductId, "Tank_productId_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.GlassType)
                .HasMaxLength(255)
                .HasColumnName("glassType");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Size)
                .HasMaxLength(255)
                .HasColumnName("size");
            entity.Property(e => e.SizeInformation)
                .HasMaxLength(255)
                .HasColumnName("sizeInformation");

            entity.HasOne(d => d.Product).WithOne(p => p.Tank)
                .HasForeignKey<Tank>(d => d.ProductId)
                .HasConstraintName("Tank_productId_fkey");
        });
    }
}