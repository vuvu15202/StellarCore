using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    public partial class RemoveLegacyRoleFunctionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleFunctions");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("38b657f4-ac20-4a5c-b2a3-16dfad61c381"),
                column: "ConcurrencyStamp",
                value: "0824120e-d3a1-44c4-b61f-8b27326a37e6");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("582880c3-f554-490f-a24e-526db35cffa5"),
                column: "ConcurrencyStamp",
                value: "4bbbf981-8ec6-496b-aa0a-c32133a0ed4a");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c4a3298c-6198-4d12-bd1a-56d1d1ce0aa7"),
                column: "ConcurrencyStamp",
                value: "caf188cb-ac35-47fb-a1ad-fb15ed05e62d");

            migrationBuilder.UpdateData(
                table: "plans",
                keyColumn: "Id",
                keyValue: new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 13, 34, 14, 562, DateTimeKind.Utc).AddTicks(1749));

            migrationBuilder.UpdateData(
                table: "plans",
                keyColumn: "Id",
                keyValue: new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 13, 34, 14, 562, DateTimeKind.Utc).AddTicks(1751));

            migrationBuilder.UpdateData(
                table: "plans",
                keyColumn: "Id",
                keyValue: new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 13, 34, 14, 562, DateTimeKind.Utc).AddTicks(1752));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleFunctions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FunctionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleFunctions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("38b657f4-ac20-4a5c-b2a3-16dfad61c381"),
                column: "ConcurrencyStamp",
                value: "ab178f44-d257-4366-b768-066ab7886c16");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("582880c3-f554-490f-a24e-526db35cffa5"),
                column: "ConcurrencyStamp",
                value: "6a389b60-c7ac-413a-b349-59f3bab03a80");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c4a3298c-6198-4d12-bd1a-56d1d1ce0aa7"),
                column: "ConcurrencyStamp",
                value: "252d5aca-4269-44ea-ac6c-5e93e5888162");

            migrationBuilder.UpdateData(
                table: "plans",
                keyColumn: "Id",
                keyValue: new Guid("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 7, 45, 21, 618, DateTimeKind.Utc).AddTicks(8723));

            migrationBuilder.UpdateData(
                table: "plans",
                keyColumn: "Id",
                keyValue: new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 7, 45, 21, 618, DateTimeKind.Utc).AddTicks(8728));

            migrationBuilder.UpdateData(
                table: "plans",
                keyColumn: "Id",
                keyValue: new Guid("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"),
                column: "CreatedAt",
                value: new DateTime(2026, 1, 19, 7, 45, 21, 618, DateTimeKind.Utc).AddTicks(8730));

            migrationBuilder.CreateIndex(
                name: "IX_RoleFunctions_FunctionId",
                table: "RoleFunctions",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleFunctions_RoleId",
                table: "RoleFunctions",
                column: "RoleId");
        }
    }
}
