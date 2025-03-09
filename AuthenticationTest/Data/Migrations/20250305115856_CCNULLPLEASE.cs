using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class CCNULLPLEASE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
        name: "CC",
        table: "CustomerCommunications",
        type: "nvarchar(max)",
        nullable: true, // ✅ This ensures CC is now nullable
        oldClrType: typeof(string),
        oldType: "nvarchar(max)");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5990b34b-e771-4d9f-903c-fcea6eae4c7f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "856f0d61-4b43-4862-b5be-a95c9f55de42");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ae7ea024-93aa-4174-a4dd-b3fac16d8e26");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
       name: "CC",
       table: "CustomerCommunications",
       type: "nvarchar(max)",
       nullable: false, // ❌ This will revert CC to NOT NULL if rolled back
       oldClrType: typeof(string),
       oldType: "nvarchar(max)",
       oldNullable: true);
        }
    }
}
