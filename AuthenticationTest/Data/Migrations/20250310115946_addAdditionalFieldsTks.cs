using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class addAdditionalFieldsTks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a3e25f2-2eff-41df-b6d9-d749c388ac76");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e8c2424-ec3a-4c27-89e4-efae81b8e582");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2039d1f-9cd2-42ee-ad75-14f27bbbb7ff");



           

            migrationBuilder.AddColumn<string>(
                name: "SiteName",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VLAN",
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
                keyValue: "07723fc3-8d3e-4788-8cfd-b68417bee906");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a06fd1ff-6e3e-49ac-9453-04a0fa3199be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3dae054-ae64-4185-94fd-8eb05fc2092c");

            migrationBuilder.DropColumn(
                name: "SiteName",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "VLAN",
                table: "Tickets");

           

           

            

           
        }
    }
}
