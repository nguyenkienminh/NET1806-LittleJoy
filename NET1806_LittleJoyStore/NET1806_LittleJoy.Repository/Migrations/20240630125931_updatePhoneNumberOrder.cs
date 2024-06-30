using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET1806_LittleJoy.Repository.Migrations
{
    /// <inheritdoc />
    public partial class updatePhoneNumberOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Order",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Order");
        }
    }
}
