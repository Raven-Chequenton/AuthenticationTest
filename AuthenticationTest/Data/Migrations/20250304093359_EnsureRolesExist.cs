using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthenticationTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnsureRolesExist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4169191e-2220-4faf-b6bd-5ef787e5b277");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e808937-b4ab-46cf-a36d-0fdc1f4d4f9f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e619dad-0319-499c-a320-940494338c58");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0de42774-9880-4c72-8f05-7aa10fa71445", null, "Admin", "ADMIN" },
                    { "19b0a815-a6ab-4ba0-ab85-9a4482b5ca22", null, "Agent", "AGENT" },
                    { "75798826-1142-495b-a6ff-983e4252ff44", null, "Client", "CLIENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0de42774-9880-4c72-8f05-7aa10fa71445");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19b0a815-a6ab-4ba0-ab85-9a4482b5ca22");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "75798826-1142-495b-a6ff-983e4252ff44");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4169191e-2220-4faf-b6bd-5ef787e5b277", null, "Client", "CLIENT" },
                    { "5e808937-b4ab-46cf-a36d-0fdc1f4d4f9f", null, "Admin", "ADMIN" },
                    { "9e619dad-0319-499c-a320-940494338c58", null, "Agent", "AGENT" }
                });
        }
    }
}
