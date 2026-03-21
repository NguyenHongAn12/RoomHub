using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoomAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Rooms");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "Address", "AvatarUrl", "ConcurrencyStamp", "CreatedAt", "DateOfBirth", "Email", "EmailConfirmed", "FullName", "Gender", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "RoleSpecificData", "SecurityStamp", "UpdatedAt", "UserName", "VerificationDate" },
                values: new object[] { "test-user-id-123", null, null, "6b655961-ff6c-4479-99cd-0cf667fb9a94", new DateTime(2026, 3, 9, 9, 58, 52, 101, DateTimeKind.Utc).AddTicks(9532), null, "owner@roomhub.com", true, "Chủ Trọ Test", null, null, "OWNER@ROOMHUB.COM", "OWNER_TEST", "AQAAAAEAACcQAAAAE...", "0123456789", null, "528cbd7e-b24c-42ff-84eb-0ed9d3c34fd8", null, "owner_test", null });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "owner-role-id", "test-user-id-123" });

            migrationBuilder.InsertData(
                table: "Buildings",
                columns: new[] { "Id", "Address", "City", "CreatedAt", "District", "Latitude", "Longitude", "Name", "OwnerId", "Province", "UpdatedAt", "Ward" },
                values: new object[] { 1, "123 Đường Test", "Hồ Chí Minh", new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quận 1", null, null, "Chung cư mini RoomHub", "test-user-id-123", null, null, "Phường Bến Nghé" });

            migrationBuilder.InsertData(
                table: "Floors",
                columns: new[] { "Id", "BuildingId", "CreatedAt", "Description", "FloorNumber" },
                values: new object[] { 1, 1, new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tầng 1 (Trệt)", 1 });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "BasePrice", "CreatedAt", "Description", "FloorId", "IsFurnished", "LandlordId", "MaxCapacity", "Photos", "RoomNumber", "RoomType", "Status", "SurfaceArea", "Title", "UpdatedAt" },
                values: new object[] { 1, 3500000m, new DateTime(2026, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phòng mới xây, dọn vào ở ngay.", 1, true, "test-user-id-123", 2, null, "101", "Other", "Available", 25.5m, "Phòng trọ cao cấp cửa sổ thoáng mát", null });

            migrationBuilder.InsertData(
                table: "RoomAmenities",
                columns: new[] { "AmenityId", "RoomId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "owner-role-id", "test-user-id-123" });

            migrationBuilder.DeleteData(
                table: "RoomAmenities",
                keyColumns: new[] { "AmenityId", "RoomId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "RoomAmenities",
                keyColumns: new[] { "AmenityId", "RoomId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Floors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Buildings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Rooms",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
