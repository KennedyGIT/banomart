using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Banomart.Services.ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryName", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1L, "Accessories", "A durable, vintage-inspired canvas backpack with ample storage for daily essentials.", "https://cdn.pixabay.com/photo/2019/02/08/15/33/journey-3983404_1280.jpg", "Vintage Canvas Backpack", 49.990000000000002 },
                    { 2L, "Home & Kitchen", "Eco-friendly, stainless steel water bottle with a leak-proof cap. Keeps drinks hot or cold for hours.", "https://cdn.pixabay.com/photo/2017/01/23/09/52/water-2001912_1280.jpg", "Stainless Steel Water Bottle", 19.989999999999998 },
                    { 3L, "Electronics", "Comfortable wireless mouse with customizable buttons and adjustable DPI for precision control.", "https://cdn.pixabay.com/photo/2017/04/07/13/13/wireless-mouse-2210970_1280.jpg", "Ergonomic Wireless Mouse", 29.949999999999999 },
                    { 4L, "Groceries", "A pack of premium, organic green tea leaves rich in antioxidants and natural flavors.", "https://cdn.pixabay.com/photo/2021/03/08/06/23/green-tea-6078275_1280.jpg", "Organic Green Tea Leaves", 15.5 },
                    { 5L, "Fitness", "Non-slip yoga mat with a comfortable thickness and a carrying strap for easy transport.", "https://cdn.pixabay.com/photo/2016/10/15/18/29/yoga-mat-1743203_1280.jpg", "Yoga Mat with Carrying Strap", 35.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
