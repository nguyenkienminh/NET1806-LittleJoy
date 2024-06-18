using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET1806_LittleJoy.Repository.Migrations
{
    /// <inheritdoc />
    public partial class fixpayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Payment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Payment",
                type: "int",
                nullable: true);
        }
    }
}
