using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthenticationTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompanyUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCompanies",
                table: "UserCompanies");

            migrationBuilder.DropIndex(
                name: "IX_UserCompanies_UserId",
                table: "UserCompanies");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9be40a89-3d68-43c5-b49e-c81ffb2679e3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d44161a9-a377-47bc-b1ad-bfad2f76389b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e90cd081-af54-458b-9996-0f658595e341");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserCompanies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCompanies",
                table: "UserCompanies",
                columns: new[] { "UserId", "CompanyId" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCompanies",
                table: "UserCompanies");

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

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserCompanies",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCompanies",
                table: "UserCompanies",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9be40a89-3d68-43c5-b49e-c81ffb2679e3", null, "Agent", "AGENT" },
                    { "d44161a9-a377-47bc-b1ad-bfad2f76389b", null, "Admin", "ADMIN" },
                    { "e90cd081-af54-458b-9996-0f658595e341", null, "Client", "CLIENT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCompanies_UserId",
                table: "UserCompanies",
                column: "UserId");
        }
    }
}
