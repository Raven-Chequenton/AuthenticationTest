using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthenticationTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6ceed78a-4daf-4fe5-8ccb-76410a371f98", null, "Client", "CLIENT" },
                    { "e2faeec9-8f12-40b2-aa38-60242105f184", null, "Admin", "ADMIN" },
                    { "e8509945-7c87-4cd1-af6a-d653a39eb2cf", null, "Agent", "AGENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6ceed78a-4daf-4fe5-8ccb-76410a371f98");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2faeec9-8f12-40b2-aa38-60242105f184");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e8509945-7c87-4cd1-af6a-d653a39eb2cf");
        }
    }
}
