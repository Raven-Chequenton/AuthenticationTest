using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthenticationTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIssueTypeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a3288a5-9cd2-4e76-b5f8-71004d82e2cc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b314493-7c47-4b1e-828e-e457591e339b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9ecc4228-623c-4850-a943-b32cc67299ca");

            migrationBuilder.CreateTable(
                name: "IssueTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IssueTypeFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FieldType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssueTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueTypeFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueTypeFields_IssueTypes_IssueTypeId",
                        column: x => x.IssueTypeId,
                        principalTable: "IssueTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1db33e82-1ad5-4da5-b47e-168ec0c7ef31", null, "Agent", "AGENT" },
                    { "790cd9c9-91cd-4c56-9f74-83ff7a0b213b", null, "Admin", "ADMIN" },
                    { "de17c486-177a-4760-90e3-243e3e40acad", null, "Client", "CLIENT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssueTypeFields_IssueTypeId",
                table: "IssueTypeFields",
                column: "IssueTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueTypeFields");

            migrationBuilder.DropTable(
                name: "IssueTypes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1db33e82-1ad5-4da5-b47e-168ec0c7ef31");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "790cd9c9-91cd-4c56-9f74-83ff7a0b213b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "de17c486-177a-4760-90e3-243e3e40acad");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8a3288a5-9cd2-4e76-b5f8-71004d82e2cc", null, "Client", "CLIENT" },
                    { "8b314493-7c47-4b1e-828e-e457591e339b", null, "Agent", "AGENT" },
                    { "9ecc4228-623c-4850-a943-b32cc67299ca", null, "Admin", "ADMIN" }
                });
        }
    }
}
