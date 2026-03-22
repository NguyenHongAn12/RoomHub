using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "115ebf74-f8b7-4e9a-9796-b272d767816d", new DateTime(2026, 3, 21, 14, 54, 36, 637, DateTimeKind.Utc).AddTicks(2812), "e4c13468-864d-41f2-8c20-f414e77134c2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "f483b2cc-b54e-4ffd-8c21-b1b8c3c8d02f", new DateTime(2026, 3, 21, 14, 53, 12, 998, DateTimeKind.Utc).AddTicks(7936), "91c114f7-8e83-440a-9fd3-c6290f24f317" });
        }
    }
}
