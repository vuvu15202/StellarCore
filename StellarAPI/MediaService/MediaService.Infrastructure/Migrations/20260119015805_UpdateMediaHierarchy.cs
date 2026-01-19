using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaService.Infrastructure.Migrations
{
    public partial class UpdateMediaHierarchy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hierarchy",
                table: "Medias",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Medias",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Medias",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoragePath",
                table: "Medias",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_ParentId",
                table: "Medias",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Medias_ParentId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "Hierarchy",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "StoragePath",
                table: "Medias");
        }
    }
}
