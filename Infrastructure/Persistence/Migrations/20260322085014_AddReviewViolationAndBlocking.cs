using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewViolationAndBlocking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewBlockedUntil",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReviewViolations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewViolations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewViolations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "ReviewBlockedUntil", "SecurityStamp" },
                values: new object[] { "eed97ed5-8f4e-46b0-b75d-b2c275a47e68", new DateTime(2026, 3, 22, 8, 50, 12, 600, DateTimeKind.Utc).AddTicks(5040), null, "0fdafeca-6e68-4bd2-93fc-6bba907bb1f2" });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewViolations_UserId",
                table: "ReviewViolations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewViolations");

            migrationBuilder.DropColumn(
                name: "ReviewBlockedUntil",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "ca4371a1-0e59-4051-9fa9-2f286c22133d", new DateTime(2026, 3, 21, 7, 45, 41, 295, DateTimeKind.Utc).AddTicks(454), "eb7d375c-9301-4aee-8140-3994eea223d3" });
        }
    }
}
