using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    public partial class UpdateFunctionGroupToListGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
