using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SparePart.ModelAndPersistance.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerContact = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerEmail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerAddress = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SupplierName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SupplierContact = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SupplierEmail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SupplierAddress = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<byte[]>(type: "longblob", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "longblob", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    WarehouseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WarehouseName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WarehouseAddress = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.WarehouseId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    PartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SKU = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PartName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SellingPrice = table.Column<double>(type: "double", nullable: false),
                    BuyingPrice = table.Column<double>(type: "double", nullable: false),
                    EntryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.PartId);
                    table.ForeignKey(
                        name: "FK_Parts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parts_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QuotationLists",
                columns: table => new
                {
                    QuoteNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuoteDate = table.Column<DateOnly>(type: "date", nullable: false),
                    QuoteValidDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<double>(type: "double", nullable: false),
                    PaymentType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationLists", x => x.QuoteNo);
                    table.ForeignKey(
                        name: "FK_QuotationLists_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuotationLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    StorageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => x.StorageId);
                    table.ForeignKey(
                        name: "FK_Storages_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Storages_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QuotationParts",
                columns: table => new
                {
                    QuotePartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuoteNo = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<double>(type: "double", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationParts", x => x.QuotePartId);
                    table.ForeignKey(
                        name: "FK_QuotationParts_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuotationParts_QuotationLists_QuoteNo",
                        column: x => x.QuoteNo,
                        principalTable: "QuotationLists",
                        principalColumn: "QuoteNo",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Engine Parts", "Parts related to vehicle engines." },
                    { 2, "Brake System", "Parts related to vehicle brake systems." }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CustomerAddress", "CustomerContact", "CustomerEmail", "CustomerName" },
                values: new object[] { 1, "123 Main Street", "1234567890", "john.doe@example.com", "John Doe" });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierId", "SupplierAddress", "SupplierContact", "SupplierEmail", "SupplierName" },
                values: new object[,]
                {
                    { 1, "456 Oak Avenue", "1234567890", "info@abcparts.com", "ABC Parts Inc." },
                    { 2, "456 Oak Avenue", "1234567890", "info@abcparts.com", "ABC Parts Inc." }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "Password", "PasswordHash", "PasswordSalt" },
                values: new object[] { 1, "rmaict@gmail.com", "qwer123", new byte[] { 24, 95, 3, 241, 181, 142, 4, 72, 26, 60, 48, 116, 217, 123, 230, 74, 83, 176, 245, 48, 92, 111, 89, 169, 223, 153, 39, 19, 242, 21, 111, 250, 60, 231, 196, 52, 26, 223, 216, 40, 253, 224, 18, 171, 10, 106, 209, 179, 65, 161, 211, 133, 37, 203, 28, 223, 187, 37, 210, 220, 190, 159, 166, 89 }, new byte[] { 169, 92, 249, 228, 30, 163, 76, 7, 232, 93, 205, 50, 179, 230, 151, 150, 93, 111, 179, 226, 221, 239, 234, 224, 64, 42, 31, 155, 132, 214, 4, 118, 240, 73, 230, 117, 12, 72, 133, 213, 217, 138, 60, 144, 196, 177, 165, 12, 0, 115, 183, 63, 83, 123, 199, 81, 16, 14, 145, 167, 212, 119, 49, 109, 141, 187, 220, 50, 101, 157, 108, 53, 145, 110, 146, 61, 216, 234, 181, 229, 250, 212, 150, 234, 210, 21, 253, 219, 163, 215, 35, 194, 74, 4, 88, 156, 216, 136, 137, 175, 240, 158, 11, 98, 36, 190, 28, 208, 68, 188, 112, 27, 44, 204, 110, 103, 91, 178, 106, 214, 14, 229, 144, 29, 180, 225, 178, 67 } });

            migrationBuilder.InsertData(
                table: "Warehouses",
                columns: new[] { "WarehouseId", "WarehouseAddress", "WarehouseName" },
                values: new object[] { 1, "789 Elm Street", "Main Warehouse" });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "PartId", "BuyingPrice", "CategoryId", "EntryDate", "PartName", "SKU", "SellingPrice", "SupplierId" },
                values: new object[] { 1, 12.0, 1, new DateOnly(2023, 1, 1), "Engine", "ENG123", 5000.0, 1 });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "PartId", "BuyingPrice", "CategoryId", "EntryDate", "PartName", "SKU", "SellingPrice", "SupplierId" },
                values: new object[] { 2, 8.0, 2, new DateOnly(2023, 1, 5), "Brake Pads", "BP456", 50.0, 2 });

            migrationBuilder.InsertData(
                table: "QuotationLists",
                columns: new[] { "QuoteNo", "CustomerId", "PaymentType", "QuoteDate", "QuoteValidDate", "TotalAmount", "UserId" },
                values: new object[] { 1, 1, "Credit", new DateOnly(2023, 2, 1), new DateOnly(2023, 2, 15), 150.0, 1 });

            migrationBuilder.InsertData(
                table: "QuotationParts",
                columns: new[] { "QuotePartId", "PartId", "Quantity", "QuoteNo", "UnitPrice" },
                values: new object[] { 1, 1, 5, 1, 20.0 });

            migrationBuilder.InsertData(
                table: "Storages",
                columns: new[] { "StorageId", "Location", "PartId", "Quantity", "WarehouseId" },
                values: new object[] { 1, "Section A", 1, 50, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Parts_CategoryId",
                table: "Parts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_SupplierId",
                table: "Parts",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLists_CustomerId",
                table: "QuotationLists",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLists_UserId",
                table: "QuotationLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationParts_PartId",
                table: "QuotationParts",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationParts_QuoteNo",
                table: "QuotationParts",
                column: "QuoteNo");

            migrationBuilder.CreateIndex(
                name: "IX_Storages_PartId",
                table: "Storages",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_Storages_WarehouseId",
                table: "Storages",
                column: "WarehouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuotationParts");

            migrationBuilder.DropTable(
                name: "Storages");

            migrationBuilder.DropTable(
                name: "QuotationLists");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Suppliers");
        }
    }
}
