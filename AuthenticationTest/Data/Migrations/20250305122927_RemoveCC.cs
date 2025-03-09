using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a207912-90b6-4682-bed5-c78962a8b39b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40d8db6e-ed24-463b-adad-78231a0d4e4f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60b97267-f84a-437b-a21d-4bff2a8e4dae");

            migrationBuilder.DropColumn(
                name: "CC",
                table: "CustomerCommunications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24591682-a130-4161-9518-810e7098a551");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5931312c-c65e-4916-bee2-3e27b1e71c0a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc450374-8fe3-4053-a5d6-49925e6baf3a");

            migrationBuilder.AddColumn<string>(
                name: "CC",
                table: "CustomerCommunications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
