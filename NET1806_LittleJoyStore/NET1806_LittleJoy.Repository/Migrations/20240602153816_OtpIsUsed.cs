using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NET1806_LittleJoy.Repository.Migrations
{
    /// <inheritdoc />
    public partial class OtpIsUsed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "Otp",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "Otp");
        }
    }
}
