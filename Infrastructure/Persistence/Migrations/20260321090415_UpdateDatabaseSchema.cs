using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "471aa790-a68b-4071-a923-a4a8de447c39", new DateTime(2026, 3, 21, 9, 4, 14, 884, DateTimeKind.Utc).AddTicks(5162), "1ef7633a-97c6-4002-80e7-044afc8afbce" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "776bac98-cc02-420c-b023-5ab9d00e36e3", new DateTime(2026, 3, 16, 9, 53, 24, 645, DateTimeKind.Utc).AddTicks(7240), "dd3873b0-7f4f-438c-8aaf-5ffba621e7a0" });
        }
    }
}
