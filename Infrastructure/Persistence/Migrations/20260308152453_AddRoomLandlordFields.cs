using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomLandlordFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Rooms",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LandlordId",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Rooms",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_LandlordId",
                table: "Rooms",
                column: "LandlordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_LandlordId",
                table: "Rooms",
                column: "LandlordId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_LandlordId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_LandlordId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "LandlordId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Rooms");
        }
    }
}
