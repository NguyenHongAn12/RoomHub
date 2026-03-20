using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomPhotosTableV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "776bac98-cc02-420c-b023-5ab9d00e36e3", new DateTime(2026, 3, 16, 9, 53, 24, 645, DateTimeKind.Utc).AddTicks(7240), "dd3873b0-7f4f-438c-8aaf-5ffba621e7a0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "08398d0b-283c-439d-8921-ce6c83f3cecb", new DateTime(2026, 3, 13, 16, 28, 41, 165, DateTimeKind.Utc).AddTicks(3086), "64a7d53c-6301-498c-a547-bb463993c1f9" });
        }
    }
}
