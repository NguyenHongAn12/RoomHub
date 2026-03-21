using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewReplies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentReviewId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "ca4371a1-0e59-4051-9fa9-2f286c22133d", new DateTime(2026, 3, 21, 7, 45, 41, 295, DateTimeKind.Utc).AddTicks(454), "eb7d375c-9301-4aee-8140-3994eea223d3" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ParentReviewId",
                table: "Reviews",
                column: "ParentReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Reviews_ParentReviewId",
                table: "Reviews",
                column: "ParentReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Reviews_ParentReviewId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ParentReviewId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ParentReviewId",
                table: "Reviews");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test-user-id-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "bcad55a5-5579-4998-ad95-e6ad584624db", new DateTime(2026, 3, 21, 5, 10, 24, 706, DateTimeKind.Utc).AddTicks(6175), "a5279342-6fa1-4be6-8655-fe01a6277608" });
        }
    }
}
