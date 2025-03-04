using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthenticationTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCircuitTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Circuits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Circuits",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6ed24943-044a-48fc-82af-84c204a9ba21", null, "Agent", "AGENT" },
                    { "aaecfb27-a0d8-496a-9e39-558133a65867", null, "Client", "CLIENT" },
                    { "fc1db45e-b652-4ff2-84e6-05e7a1e77f2a", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6ed24943-044a-48fc-82af-84c204a9ba21");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aaecfb27-a0d8-496a-9e39-558133a65867");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc1db45e-b652-4ff2-84e6-05e7a1e77f2a");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Circuits");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Circuits");

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
    }
}
