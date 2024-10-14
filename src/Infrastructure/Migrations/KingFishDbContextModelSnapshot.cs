﻿// <auto-generated />
using System;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(KingFishDbContext))]
    partial class KingFishDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CategoryTank", b =>
                {
                    b.Property<Guid>("CategoriesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TanksId")
                        .HasColumnType("uuid");

                    b.HasKey("CategoriesId", "TanksId");

                    b.HasIndex("TanksId");

                    b.ToTable("CategoryTank");
                });

            modelBuilder.Entity("Domain.Entites.Blog", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .HasColumnType("character varying")
                        .HasColumnName("content");

                    b.Property<string>("ContentHtml")
                        .HasColumnType("character varying")
                        .HasColumnName("contentHtml");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("createdAt");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deletedAt");

                    b.Property<string>("Slug")
                        .HasColumnType("character varying")
                        .HasColumnName("slug");

                    b.Property<Guid?>("SupplierId")
                        .HasColumnType("uuid")
                        .HasColumnName("supplierId");

                    b.Property<string>("Title")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("title");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updatedAt");

                    b.HasKey("Id")
                        .HasName("Blog_pkey");

                    b.HasIndex(new[] { "SupplierId" }, "IX_Blog_supplierId");

                    b.ToTable("Blog", (string)null);
                });

            modelBuilder.Entity("Domain.Entites.Breed", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("createdAt");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deletedAt");

                    b.Property<string>("Description")
                        .HasColumnType("character varying")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updatedAt");

                    b.HasKey("Id")
                        .HasName("Breed_pkey");

                    b.ToTable("Breed", (string)null);
                });

            modelBuilder.Entity("Domain.Entites.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("createdAt");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deletedAt");

                    b.Property<string>("Level")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("level");

                    b.Property<string>("TankType")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("TankType");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updatedAt");

                    b.HasKey("Id")
                        .HasName("Category_pkey");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("Domain.Entites.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("BlogId")
                        .HasColumnType("uuid")
                        .HasColumnName("blogId");

                    b.Property<string>("Content")
                        .HasColumnType("character varying")
                        .HasColumnName("content");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("createdAt");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uuid")
                        .HasColumnName("customerId");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deletedAt");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updatedAt");

                    b.HasKey("Id")
                        .HasName("Comment_pkey");

                    b.HasIndex(new[] { "BlogId" }, "IX_Comment_blogId");

                    b.HasIndex(new[] { "CustomerId" }, "IX_Comment_customerId");

                    b.ToTable("Comment", (string)null);
                });

            modelBuilder.Entity("Domain.Entites.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .HasColumnType("character varying")
                        .HasColumnName("address");

                    b.Property<DateOnly?>("Birthday")
                        .HasColumnType("date")
                        .HasColumnName("birthday");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("createdAt");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deletedAt");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<string>("Gender")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("gender");

                    b.Property<int?>("LoyaltyPoints")
                        .HasColumnType("integer")
                        .HasColumnName("loyaltyPoints");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("phone");

                    b.Property<DateOnly?>("RegistrationDate")
                        .HasColumnType("date")
                        .HasColumnName("registrationDate");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updatedAt");

                    b.HasKey("Id")
                        .HasName("Customer_pkey");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("Domain.Entites.Feedback", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .HasColumnType("character varying")
                        .HasColumnName("content");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("createdAt");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deletedAt");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("productId");

                    b.Property<int?>("Rate")
                        .HasColumnType("integer")
                        .HasColumnName("rate");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updatedAt");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("userId");

                    b.HasKey("Id")
                        .HasName("Feedback_pkey");

                    b.HasIndex(new[] { "ProductId" }, "IX_Feedback_productId");

                    b.HasIndex(new[] { "UserId" }, "IX_Feedback_userId");

                    b.ToTable("Feedback", (string)null);
                });

            modelBuilder.Entity("Domain.Entites.Fish", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Age")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("age");

                    b.Property<Guid?>("BreedId")
                        .HasColumnType("uuid")
                        .HasColumnName("breedId");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date")
                        .HasColumnName("dateOfBirth");

                    b.Property<string>("FoodAmount")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("foodAmount");

                    b.Property<string>("Health")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("health");

                    b.Property<string>("Origin")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("origin");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("productId");

                    b.Property<string>("Sex")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("sex");

                    b.Property<string>("Size")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("size");

                    b.Property<decimal?>("Weight")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("weight");

                    b.HasKey("Id")
                        .HasName("Fish_pkey");

                    b.HasIndex("ProductId")
                        .IsUnique();

                    b.HasIndex(new[] { "BreedId" }, "IX_Fish_breedId");

                    b.HasIndex(new[] { "ProductId" }, "IX_Fish_productId")
                        .HasDatabaseName("IX_Fish_productId1");

                    b.ToTable("Fish", (string)null);
                });

            modelBuilder.Entity("Domain.Entites.FishAward", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateOnly>("AwardDate")
                        .HasColumnType("date")
                        .HasColumnName("awardDate");

                    b.Property<string>("Description")
                        .HasColumnType("character varying")
                        .HasColumnName("description");

                    b.Property<Guid?>("FishId")
                        .HasColumnType("uuid")
                        .HasColumnName("fishId");

                    b.Property<string>("Image")
                        .HasColumnType("character varying")
                        .HasColumnName("image");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("FishAward_pkey");

                    b.HasIndex(new[] { "FishId" }, "IX_FishAward_fishId");

                    b.ToTable("FishAward", (string)null);
                });

            modelBuilder.Entity("Domain.Entites.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("BlogId")
                        .HasColumnType("uuid")
                        .HasColumnName("blogId");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("createdAt");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deletedAt");

                    b.Property<string>("Link")
                        .HasColumnType("character varying")
                        .HasColumnName("cloudLink");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("productId");

                    b.Property<string>("PublicId")
                        .HasColumnType("character varying")
                        .HasColumnName("publicId");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updatedAt");

                    b.HasKey("Id")
                        .HasName("Image_pkey");

                    b.HasIndex(new[] { "BlogId" }, "IX_Image_blogId");

                    b.HasIndex(new[] { "ProductId" }, "IX_Image_productId");

                    b.ToTable("Image", (string)null);
                });

            modelBuilder.Entity("Domain.Entites.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("createdAt");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uuid")
                        .HasColumnName("customerId");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deletedAt");

                    b.Property<int?>("OrderCode")
                        .HasColumnType("integer")
                        .HasColumnName("orderCode");

                    b.Property<DateTime?>("OrderDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("orderDate");

                    b.Property<string>("PaymentMethod")
                        .HasColumnType("character varying")
                        .HasColumnName("paymentMethod");

                    b.Property<string>("ShipAddress")
                        .HasColumnType("character varying")
                        .HasColumnName("shipAddress");

                    b.Property<DateTime?>("ShippedDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("shippedDate");

                    b.Property<string>("Status")
                        .HasColumnType("character varying")
                        .HasColumnName("status");

                    b.Property<decimal?>("TotalPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("totalPrice");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updatedAt");

                    b.HasKey("Id")
                        .HasName("Orders_pkey");

                    b.HasIndex(new[] { "CustomerId" }, "IX_Orders_customerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Domain.Entites.OrderDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<decimal?>("Discount")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("discount");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("orderId");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("productId");

                    b.Property<int?>("Quantity")
                        .HasColumnType("integer")
                        .HasColumnName("quantity");

                    b.Property<decimal?>("TotalPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("totalPrice");

                    b.Property<decimal?>("UnitPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("unitPrice");

                    b.HasKey("Id")
                        .HasName("OrderDetail_pkey");

                    b.HasIndex(new[] { "OrderId" }, "IX_OrderDetail_orderId");

                    b.HasIndex(new[] { "ProductId" }, "IX_OrderDetail_productId");

                    b.ToTable("OrderDetail", (string)null);
                });

            modelBuilder.Entity("Domain.Entites.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("createdAt");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deletedAt");

                    b.Property<string>("Description")
                        .HasColumnType("character varying")
                        .HasColumnName("description");

                    b.Property<string>("DescriptionDetail")
                        .HasColumnType("character varying")
                        .HasColumnName("descriptionDetail");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<decimal?>("OriginalPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("orginalPrice");

                    b.Property<decimal?>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("price");

                    b.Property<string>("Slug")
                        .HasColumnType("character varying")
                        .HasColumnName("slug");

                    b.Property<int?>("Sold")
                        .HasColumnType("integer")
                        .HasColumnName("sold");

                    b.Property<int?>("StockQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("stockQuantity");

                    b.Property<Guid?>("SupplierId")
                        .HasColumnType("uuid")
                        .HasColumnName("supplierId");

                    b.Property<string>("Type")
                        .HasColumnType("character varying")
                        .HasColumnName("Type");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updatedAt");

                    b.HasKey("Id")
                        .HasName("Product_pkey");

                    b.HasIndex(new[] { "SupplierId" }, "IX_Product_supplierId");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("Domain.Entites.Supplier", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AddressStore")
                        .HasColumnType("character varying")
                        .HasColumnName("addressStore");

                    b.Property<string>("CompanyName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("companyName");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("createdAt");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("deletedAt");

                    b.Property<string>("Facebook")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("facebook");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updatedAt");

                    b.Property<string>("Username")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("Supplier_pkey");

                    b.ToTable("Supplier", (string)null);
                });

            modelBuilder.Entity("Domain.Entites.Tank", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("GlassType")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("glassType");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("productId");

                    b.Property<string>("Size")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("size");

                    b.Property<string>("SizeInformation")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("sizeInformation");

                    b.HasKey("Id")
                        .HasName("Tank_pkey");

                    b.HasIndex(new[] { "ProductId" }, "Tank_productId_key")
                        .IsUnique();

                    b.ToTable("Tank", (string)null);
                });

            modelBuilder.Entity("CategoryTank", b =>
                {
                    b.HasOne("Domain.Entites.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entites.Tank", null)
                        .WithMany()
                        .HasForeignKey("TanksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entites.Blog", b =>
                {
                    b.HasOne("Domain.Entites.Supplier", "Supplier")
                        .WithMany("Blogs")
                        .HasForeignKey("SupplierId")
                        .HasConstraintName("Blog_supplierId_fkey");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("Domain.Entites.Comment", b =>
                {
                    b.HasOne("Domain.Entites.Blog", "Blog")
                        .WithMany("Comments")
                        .HasForeignKey("BlogId")
                        .HasConstraintName("Comment_blogId_fkey");

                    b.HasOne("Domain.Entites.Customer", "Customer")
                        .WithMany("Comments")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("Comment_customerId_fkey");

                    b.Navigation("Blog");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Domain.Entites.Feedback", b =>
                {
                    b.HasOne("Domain.Entites.Product", "Product")
                        .WithMany("Feedbacks")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("Feedback_productId_fkey");

                    b.HasOne("Domain.Entites.Customer", "User")
                        .WithMany("Feedbacks")
                        .HasForeignKey("UserId")
                        .HasConstraintName("Feedback_userId_fkey");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entites.Fish", b =>
                {
                    b.HasOne("Domain.Entites.Breed", "Breed")
                        .WithMany("Fishes")
                        .HasForeignKey("BreedId")
                        .HasConstraintName("Fish_breedId_fkey");

                    b.HasOne("Domain.Entites.Product", "Product")
                        .WithOne("Fish")
                        .HasForeignKey("Domain.Entites.Fish", "ProductId")
                        .HasConstraintName("Fish_productId_fkey");

                    b.Navigation("Breed");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Domain.Entites.FishAward", b =>
                {
                    b.HasOne("Domain.Entites.Fish", "Fish")
                        .WithMany("Awards")
                        .HasForeignKey("FishId")
                        .HasConstraintName("FishAward_fishId_fkey");

                    b.Navigation("Fish");
                });

            modelBuilder.Entity("Domain.Entites.Image", b =>
                {
                    b.HasOne("Domain.Entites.Blog", "Blog")
                        .WithMany("Images")
                        .HasForeignKey("BlogId")
                        .HasConstraintName("Image_blogId_fkey");

                    b.HasOne("Domain.Entites.Product", "Product")
                        .WithMany("Images")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("Image_productId_fkey");

                    b.Navigation("Blog");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Domain.Entites.Order", b =>
                {
                    b.HasOne("Domain.Entites.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("Orders_customerId_fkey");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Domain.Entites.OrderDetail", b =>
                {
                    b.HasOne("Domain.Entites.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .HasConstraintName("OrderDetail_orderId_fkey");

                    b.HasOne("Domain.Entites.Product", "Product")
                        .WithMany("OrderDetails")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("OrderDetail_productId_fkey");

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Domain.Entites.Product", b =>
                {
                    b.HasOne("Domain.Entites.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .HasConstraintName("Product_supplierId_fkey");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("Domain.Entites.Tank", b =>
                {
                    b.HasOne("Domain.Entites.Product", "Product")
                        .WithOne("Tank")
                        .HasForeignKey("Domain.Entites.Tank", "ProductId")
                        .HasConstraintName("Tank_productId_fkey");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Domain.Entites.Blog", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("Domain.Entites.Breed", b =>
                {
                    b.Navigation("Fishes");
                });

            modelBuilder.Entity("Domain.Entites.Customer", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Feedbacks");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Domain.Entites.Fish", b =>
                {
                    b.Navigation("Awards");
                });

            modelBuilder.Entity("Domain.Entites.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("Domain.Entites.Product", b =>
                {
                    b.Navigation("Feedbacks");

                    b.Navigation("Fish");

                    b.Navigation("Images");

                    b.Navigation("OrderDetails");

                    b.Navigation("Tank");
                });

            modelBuilder.Entity("Domain.Entites.Supplier", b =>
                {
                    b.Navigation("Blogs");

                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
