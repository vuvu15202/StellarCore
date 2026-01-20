using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    public partial class AddFunctionGroupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FunctionGroupId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "function_groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultFunctionIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultFunctionGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RuleIgnoreFunctionIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RuleOnlyViewCreatedBy = table.Column<bool>(type: "bit", nullable: false),
                    RuleViewFunctionGroupId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_function_groups", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("38b657f4-ac20-4a5c-b2a3-16dfad61c381"),
                column: "ConcurrencyStamp",
                value: "7b8b4909-309f-42ec-9541-af5666010277");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("582880c3-f554-490f-a24e-526db35cffa5"),
                column: "ConcurrencyStamp",
                value: "d24b6fcb-5800-4fca-bc51-afd388bce7db");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c4a3298c-6198-4d12-bd1a-56d1d1ce0aa7"),
                column: "ConcurrencyStamp",
                value: "aa2a85ec-4e7d-441f-b057-1c05b275cc01");

            migrationBuilder.UpdateData(
                table: "plans",
                keyColumn: "Id",
                keyValue: new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 7, 34, 15, 584, DateTimeKind.Utc).AddTicks(5469));

            migrationBuilder.UpdateData(
                table: "plans",
                keyColumn: "Id",
                keyValue: new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 7, 34, 15, 584, DateTimeKind.Utc).AddTicks(5473));

            migrationBuilder.UpdateData(
                table: "plans",
                keyColumn: "Id",
                keyValue: new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 7, 34, 15, 584, DateTimeKind.Utc).AddTicks(5474));

            migrationBuilder.CreateIndex(
                name: "IX_Users_FunctionGroupId",
                table: "Users",
                column: "FunctionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_function_groups_DefaultFunctionGroupId",
                table: "function_groups",
                column: "DefaultFunctionGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "function_groups");

            migrationBuilder.DropIndex(
                name: "IX_Users_FunctionGroupId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FunctionGroupId",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("38b657f4-ac20-4a5c-b2a3-16dfad61c381"),
                column: "ConcurrencyStamp",
                value: "fb786f72-506a-43a0-ab71-f06e8eabf511");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("582880c3-f554-490f-a24e-526db35cffa5"),
                column: "ConcurrencyStamp",
                value: "c96da6cc-efe8-49e6-9dfe-c53a2216eb95");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c4a3298c-6198-4d12-bd1a-56d1d1ce0aa7"),
                column: "ConcurrencyStamp",
                value: "c354ae82-f289-46a2-a65a-64c5a89fce0f");

            migrationBuilder.UpdateData(
                table: "plans",
                keyColumn: "Id",
                keyValue: new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 17, 17, 2, 31, 74, DateTimeKind.Utc).AddTicks(2248));

            migrationBuilder.UpdateData(
                table: "plans",
                keyColumn: "Id",
                keyValue: new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 17, 17, 2, 31, 74, DateTimeKind.Utc).AddTicks(2252));

            migrationBuilder.UpdateData(
                table: "plans",
                keyColumn: "Id",
                keyValue: new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 17, 17, 2, 31, 74, DateTimeKind.Utc).AddTicks(2254));
        }
    }
}
