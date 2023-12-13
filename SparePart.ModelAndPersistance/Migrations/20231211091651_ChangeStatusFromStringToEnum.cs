using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SparePart.ModelAndPersistance.Migrations
{
    public partial class ChangeStatusFromStringToEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "QuotationLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "QuotationLists",
                keyColumn: "QuoteNo",
                keyValue: 1,
                column: "Status",
                value: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "QuotationLists",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "QuotationLists",
                keyColumn: "QuoteNo",
                keyValue: 1,
                column: "Status",
                value: "pending");
        }
    }
}
