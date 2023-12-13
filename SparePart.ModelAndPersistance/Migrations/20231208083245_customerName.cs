using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SparePart.ModelAndPersistance.Migrations
{
    public partial class customerName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "QuotationLists",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "QuotationLists",
                keyColumn: "QuoteNo",
                keyValue: 1,
                column: "CustomerName",
                value: "John Doe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "QuotationLists");
        }
    }
}
