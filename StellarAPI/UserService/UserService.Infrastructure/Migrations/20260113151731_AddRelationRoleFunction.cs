using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    public partial class AddRelationRoleFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleFunctions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FunctionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                value: "e92a12bf-d8fb-480f-a07a-0db3b8c918a0");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("582880c3-f554-490f-a24e-526db35cffa5"),
                column: "ConcurrencyStamp",
                value: "7b0d0b4c-35c7-4976-a284-4a4e645f0be2");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c4a3298c-6198-4d12-bd1a-56d1d1ce0aa7"),
                column: "ConcurrencyStamp",
                value: "44fa31ce-ccac-4032-8c7c-2f2d5ab8c9c7");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleFunctions");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("38b657f4-ac20-4a5c-b2a3-16dfad61c381"),
                column: "ConcurrencyStamp",
                value: "d50e3713-d5d8-402f-b73c-6ad9473674c3");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("582880c3-f554-490f-a24e-526db35cffa5"),
                column: "ConcurrencyStamp",
                value: "0145ea38-c299-452c-aea9-12482927f606");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c4a3298c-6198-4d12-bd1a-56d1d1ce0aa7"),
                column: "ConcurrencyStamp",
                value: "c2858c04-f128-4052-a985-0718a4241190");
        }
    }
}
