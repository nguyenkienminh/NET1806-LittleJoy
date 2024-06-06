using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET1806_LittleJoy.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Fixdelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Order__Id__693CA210",
                table: "Order");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryStatus",
                table: "Order",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryStatus",
                table: "Order");

            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Delivery__C3905BCFBC21537C", x => x.OrderId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK__Order__Id__693CA210",
                table: "Order",
                column: "Id",
                principalTable: "Delivery",
                principalColumn: "OrderId");
        }
    }
}
