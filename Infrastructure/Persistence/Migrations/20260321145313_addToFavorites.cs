using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addToFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "f483b2cc-b54e-4ffd-8c21-b1b8c3c8d02f", new DateTime(2026, 3, 21, 14, 53, 12, 998, DateTimeKind.Utc).AddTicks(7936), "91c114f7-8e83-440a-9fd3-c6290f24f317" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "b6f0d255-b5c8-4d6b-b14a-6b8047d78e18", new DateTime(2026, 3, 21, 14, 51, 21, 824, DateTimeKind.Utc).AddTicks(4111), "0ca741f0-4200-4171-aeb0-52beb7b6262a" });
        }
    }
}
