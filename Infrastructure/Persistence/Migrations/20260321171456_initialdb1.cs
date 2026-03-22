using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initialdb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "91adf4fc-0fe3-4425-a0c0-ecce30321589", new DateTime(2026, 3, 21, 17, 14, 55, 540, DateTimeKind.Utc).AddTicks(2281), "42eab8d4-4552-4b20-9e87-666ed70201d3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "ca4371a1-0e59-4051-9fa9-2f286c22133d", new DateTime(2026, 3, 21, 7, 45, 41, 295, DateTimeKind.Utc).AddTicks(454), "eb7d375c-9301-4aee-8140-3994eea223d3" });
        }
    }
}
