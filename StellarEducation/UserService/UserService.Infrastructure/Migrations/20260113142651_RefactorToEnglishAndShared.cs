using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    public partial class RefactorToEnglishAndShared : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "functions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    SortPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HierarchyPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_functions", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "functions");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("38b657f4-ac20-4a5c-b2a3-16dfad61c381"),
                column: "ConcurrencyStamp",
                value: "d0c252ab-b10f-4aa2-a5e8-1d317d705d62");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("582880c3-f554-490f-a24e-526db35cffa5"),
                column: "ConcurrencyStamp",
                value: "7f9c8733-5976-410e-853b-7c9c75ae0045");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c4a3298c-6198-4d12-bd1a-56d1d1ce0aa7"),
                column: "ConcurrencyStamp",
                value: "c090759b-bf36-4930-b042-f0dc41a3834d");
        }
    }
}
