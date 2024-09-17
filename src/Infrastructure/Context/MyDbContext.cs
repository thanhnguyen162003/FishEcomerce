using FishEcomerce.Entities;
using Microsoft.EntityFrameworkCore;

namespace FishEcomerce.Infrastructure.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Breed> Breeds { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Fishaward> Fishawards { get; set; }

    public virtual DbSet<Fishproduct> Fishproducts { get; set; }

    public virtual DbSet<Fishtank> Fishtanks { get; set; }

    public virtual DbSet<Fishtankcategory> Fishtankcategories { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseNpgsql("Host=20.205.21.96;Port=5432;Database=kingfish;Username=root;Password=Admin123456789@");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("blog_pkey");

            entity.ToTable("blog");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Contenthtml).HasColumnName("contenthtml");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Updatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.Supplierid)
                .HasConstraintName("blog_supplierid_fkey");
        });

        modelBuilder.Entity<Breed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("breed_pkey");

            entity.ToTable("breed");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Fishproductid).HasColumnName("fishproductid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Fishproduct).WithMany(p => p.Breeds)
                .HasForeignKey(d => d.Fishproductid)
                .HasConstraintName("breed_fishproductid_fkey");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comment_pkey");

            entity.ToTable("comment");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Blogid).HasColumnName("blogid");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Createdat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Updatedat)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updatedat");

            entity.HasOne(d => d.Blog).WithMany(p => p.Comments)
                .HasForeignKey(d => d.Blogid)
                .HasConstraintName("comment_blogid_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Comments)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("comment_customerid_fkey");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_pkey");

            entity.ToTable("customer");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.Loyaltypoints).HasColumnName("loyaltypoints");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Registrationdate).HasColumnName("registrationdate");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("feedback_pkey");

            entity.ToTable("feedback");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Product).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("feedback_productid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("feedback_userid_fkey");
        });

        modelBuilder.Entity<Fishaward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fishaward_pkey");

            entity.ToTable("fishaward");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Fishproductid).HasColumnName("fishproductid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Fishproduct).WithMany(p => p.Fishawards)
                .HasForeignKey(d => d.Fishproductid)
                .HasConstraintName("fishaward_fishproductid_fkey");
        });

        modelBuilder.Entity<Fishproduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fishproduct_pkey");

            entity.ToTable("fishproduct");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Age)
                .HasMaxLength(255)
                .HasColumnName("age");
            entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Fishtype)
                .HasMaxLength(255)
                .HasColumnName("fishtype");
            entity.Property(e => e.Foodamount)
                .HasMaxLength(255)
                .HasColumnName("foodamount");
            entity.Property(e => e.Health)
                .HasMaxLength(255)
                .HasColumnName("health");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Origin)
                .HasMaxLength(255)
                .HasColumnName("origin");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Sex)
                .HasMaxLength(50)
                .HasColumnName("sex");
            entity.Property(e => e.Size)
                .HasMaxLength(255)
                .HasColumnName("size");
            entity.Property(e => e.Weight)
                .HasPrecision(18, 2)
                .HasColumnName("weight");

            entity.HasOne(d => d.Product).WithMany(p => p.Fishproducts)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("fishproduct_productid_fkey");
        });

        modelBuilder.Entity<Fishtank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fishtank_pkey");

            entity.ToTable("fishtank");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Glasstype)
                .HasMaxLength(255)
                .HasColumnName("glasstype");
            entity.Property(e => e.Informationdetail).HasColumnName("informationdetail");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Size)
                .HasMaxLength(255)
                .HasColumnName("size");
            entity.Property(e => e.Sizeinformation)
                .HasMaxLength(255)
                .HasColumnName("sizeinformation");

            entity.HasOne(d => d.Product).WithMany(p => p.Fishtanks)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("fishtank_productid_fkey");
        });

        modelBuilder.Entity<Fishtankcategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fishtankcategory_pkey");

            entity.ToTable("fishtankcategory");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Fishtankcategorytype)
                .HasMaxLength(255)
                .HasColumnName("fishtankcategorytype");
            entity.Property(e => e.Fishtankid).HasColumnName("fishtankid");
            entity.Property(e => e.Fishtanklevel)
                .HasMaxLength(255)
                .HasColumnName("fishtanklevel");

            entity.HasOne(d => d.Fishtank).WithMany(p => p.Fishtankcategories)
                .HasForeignKey(d => d.Fishtankid)
                .HasConstraintName("fishtankcategory_fishtankid_fkey");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("image_pkey");

            entity.ToTable("image");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Blogid).HasColumnName("blogid");
            entity.Property(e => e.Cloudlink).HasColumnName("cloudlink");
            entity.Property(e => e.Productid).HasColumnName("productid");

            entity.HasOne(d => d.Blog).WithMany(p => p.Images)
                .HasForeignKey(d => d.Blogid)
                .HasConstraintName("image_blogid_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.Images)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("image_productid_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Orderdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("orderdate");
            entity.Property(e => e.Shipaddress).HasColumnName("shipaddress");
            entity.Property(e => e.Shippeddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("shippeddate");
            entity.Property(e => e.Totalprice)
                .HasPrecision(18, 2)
                .HasColumnName("totalprice");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("orders_customerid_fkey");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orderdetail_pkey");

            entity.ToTable("orderdetail");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Unitprice)
                .HasPrecision(18, 2)
                .HasColumnName("unitprice");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("orderdetail_orderid_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("orderdetail_productid_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(18, 2)
                .HasColumnName("price");
            entity.Property(e => e.Productspecificationid).HasColumnName("productspecificationid");
            entity.Property(e => e.Sold).HasColumnName("sold");
            entity.Property(e => e.Stockquantity).HasColumnName("stockquantity");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.Supplierid)
                .HasConstraintName("product_supplierid_fkey");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("supplier_pkey");

            entity.ToTable("supplier");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Addressstore).HasColumnName("addressstore");
            entity.Property(e => e.Companyname)
                .HasMaxLength(255)
                .HasColumnName("companyname");
            entity.Property(e => e.Facebook)
                .HasMaxLength(255)
                .HasColumnName("facebook");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
