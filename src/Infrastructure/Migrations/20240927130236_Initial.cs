using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Breed",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Breed_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    TankType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    level = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Category_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    gender = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    birthday = table.Column<DateOnly>(type: "date", nullable: true),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    address = table.Column<string>(type: "character varying", nullable: true),
                    loyaltyPoints = table.Column<int>(type: "integer", nullable: true),
                    registrationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Customer_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    companyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    addressStore = table.Column<string>(type: "character varying", nullable: true),
                    facebook = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Supplier_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customerId = table.Column<Guid>(type: "uuid", nullable: true),
                    orderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    shippedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    totalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    status = table.Column<string>(type: "character varying", nullable: true),
                    paymentMethod = table.Column<string>(type: "character varying", nullable: true),
                    shipAddress = table.Column<string>(type: "character varying", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Orders_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Orders_customerId_fkey",
                        column: x => x.customerId,
                        principalTable: "Customer",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    slug = table.Column<string>(type: "character varying", nullable: true),
                    content = table.Column<string>(type: "character varying", nullable: true),
                    contentHtml = table.Column<string>(type: "character varying", nullable: true),
                    supplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Blog_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Blog_supplierId_fkey",
                        column: x => x.supplierId,
                        principalTable: "Supplier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    slug = table.Column<string>(type: "character varying", nullable: true),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    descriptionDetail = table.Column<string>(type: "character varying", nullable: true),
                    Type = table.Column<string>(type: "character varying", nullable: true),
                    supplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    stockQuantity = table.Column<int>(type: "integer", nullable: true),
                    sold = table.Column<int>(type: "integer", nullable: true),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    orginalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Product_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Product_supplierId_fkey",
                        column: x => x.supplierId,
                        principalTable: "Supplier",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "character varying", nullable: true),
                    blogId = table.Column<Guid>(type: "uuid", nullable: true),
                    customerId = table.Column<Guid>(type: "uuid", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Comment_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Comment_blogId_fkey",
                        column: x => x.blogId,
                        principalTable: "Blog",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Comment_customerId_fkey",
                        column: x => x.customerId,
                        principalTable: "Customer",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    productId = table.Column<Guid>(type: "uuid", nullable: true),
                    userId = table.Column<Guid>(type: "uuid", nullable: true),
                    content = table.Column<string>(type: "character varying", nullable: true),
                    rate = table.Column<int>(type: "integer", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Feedback_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Feedback_productId_fkey",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Feedback_userId_fkey",
                        column: x => x.userId,
                        principalTable: "Customer",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Fish",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    productId = table.Column<Guid>(type: "uuid", nullable: true),
                    BreedId = table.Column<Guid>(type: "uuid", nullable: true),
                    size = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    age = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    origin = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    sex = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    foodAmount = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    weight = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    health = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    dateOfBirth = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Fish_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Fish_breedId_fkey",
                        column: x => x.productId,
                        principalTable: "Breed",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Fish_productId_fkey",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cloudLink = table.Column<string>(type: "character varying", nullable: true),
                    productId = table.Column<Guid>(type: "uuid", nullable: true),
                    blogId = table.Column<Guid>(type: "uuid", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    deletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Image_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Image_blogId_fkey",
                        column: x => x.blogId,
                        principalTable: "Blog",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Image_productId_fkey",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    orderId = table.Column<Guid>(type: "uuid", nullable: true),
                    productId = table.Column<Guid>(type: "uuid", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: true),
                    unitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    totalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    discount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("OrderDetail_pkey", x => x.id);
                    table.ForeignKey(
                        name: "OrderDetail_orderId_fkey",
                        column: x => x.orderId,
                        principalTable: "Orders",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "OrderDetail_productId_fkey",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Tank",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    productId = table.Column<Guid>(type: "uuid", nullable: true),
                    size = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    sizeInformation = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    glassType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tank_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Tank_productId_fkey",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "FishAward",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fishId = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "character varying", nullable: true),
                    awardDate = table.Column<DateOnly>(type: "date", nullable: false),
                    image = table.Column<string>(type: "character varying", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FishAward_pkey", x => x.id);
                    table.ForeignKey(
                        name: "FishAward_fishId_fkey",
                        column: x => x.fishId,
                        principalTable: "Fish",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CategoryTank",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    TanksId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTank", x => new { x.CategoriesId, x.TanksId });
                    table.ForeignKey(
                        name: "FK_CategoryTank_Category_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTank_Tank_TanksId",
                        column: x => x.TanksId,
                        principalTable: "Tank",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blog_supplierId",
                table: "Blog",
                column: "supplierId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTank_TanksId",
                table: "CategoryTank",
                column: "TanksId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_blogId",
                table: "Comment",
                column: "blogId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_customerId",
                table: "Comment",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_productId",
                table: "Feedback",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_userId",
                table: "Feedback",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Fish_productId",
                table: "Fish",
                column: "productId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fish_productId1",
                table: "Fish",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_FishAward_fishId",
                table: "FishAward",
                column: "fishId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_blogId",
                table: "Image",
                column: "blogId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_productId",
                table: "Image",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_orderId",
                table: "OrderDetail",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_productId",
                table: "OrderDetail",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_customerId",
                table: "Orders",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_supplierId",
                table: "Product",
                column: "supplierId");

            migrationBuilder.CreateIndex(
                name: "Tank_productId_key",
                table: "Tank",
                column: "productId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTank");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "FishAward");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Tank");

            migrationBuilder.DropTable(
                name: "Fish");

            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Breed");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Supplier");
        }
    }
}
