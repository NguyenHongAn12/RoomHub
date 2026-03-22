using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CommentSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "bcad55a5-5579-4998-ad95-e6ad584624db", new DateTime(2026, 3, 21, 5, 10, 24, 706, DateTimeKind.Utc).AddTicks(6175), "a5279342-6fa1-4be6-8655-fe01a6277608" });
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
