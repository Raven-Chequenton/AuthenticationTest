using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketHistoryColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "407ba521-8726-4746-a4a8-84e33c03e7ad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b34a530f-b707-4934-ac07-5b2a64b54c8f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "faae909a-d436-43d8-9ac7-c53f65d5d7c5");

            migrationBuilder.AddColumn<string>(
                name: "CustomerCommunicationHistory",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalNotesHistory",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "CustomerCommunicationHistory",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "InternalNotesHistory",
                table: "Tickets");
        }
    }
}
