using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeUploadedByNullables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
       name: "UploadedBy",
       table: "TicketAttachments",
       nullable: false, // Keep NOT NULL
       defaultValue: "System", // Default to "System"
       oldClrType: typeof(string),
       oldType: "nvarchar(max)");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d1008588-81f3-47a6-a6ee-91dc45bd95b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d59d2b97-650e-4162-96c8-cfec3d5c1d37");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d8a2d6d4-f4d0-49ff-ad35-8d6f40db1d08");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5124cb37-a16a-41c6-8648-11da24775c73");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bae6bdd7-55ce-4ff8-8ba2-c72506f329bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb005158-01cf-44d3-9a10-98296ecc22be");
        }
    }
}
