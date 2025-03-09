using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixTicketRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Companies_CompanyId",
                table: "Tickets");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3679ba41-5e4e-4a3f-a5a9-f1b19d8ffa9f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7378a728-68f0-4816-adbe-ae4f6a3d328e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b1fbf7b-7418-4fcf-896c-8704118a8027");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Companies_CompanyId",
                table: "Tickets",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Companies_CompanyId",
                table: "Tickets");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d7497c8-3079-4def-81c2-d43418ca2880");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "536d8f0f-2014-4f5b-87c4-cb380c5b3d98");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac28d8aa-7828-4a46-acdf-294684c34f83");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Companies_CompanyId",
                table: "Tickets",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
