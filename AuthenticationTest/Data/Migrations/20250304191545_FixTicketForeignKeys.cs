using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixTicketForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Companies_CompanyId",
                table: "Tickets");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4a473a0d-00df-4112-b57e-cd446193e30c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea173703-7dd4-42c6-9707-56cd4303cd4d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec6914d6-7027-47df-b888-32debf7205cb");

            migrationBuilder.RenameColumn(
                name: "Requestor",
                table: "Tickets",
                newName: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "TicketRef",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CircuitId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IssueTypeId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RequestorId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "SLAExpiration",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SLA",
                table: "Circuits",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CircuitId",
                table: "Tickets",
                column: "CircuitId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_IssueTypeId",
                table: "Tickets",
                column: "IssueTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_RequestorId",
                table: "Tickets",
                column: "RequestorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_RequestorId",
                table: "Tickets",
                column: "RequestorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Circuits_CircuitId",
                table: "Tickets",
                column: "CircuitId",
                principalTable: "Circuits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Companies_CompanyId",
                table: "Tickets",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_IssueTypes_IssueTypeId",
                table: "Tickets",
                column: "IssueTypeId",
                principalTable: "IssueTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_RequestorId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Circuits_CircuitId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Companies_CompanyId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_IssueTypes_IssueTypeId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_CircuitId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_IssueTypeId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_RequestorId",
                table: "Tickets");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "28090e9a-35a3-457f-a146-9076a937c18d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e8202ea9-a41d-43e0-a90e-9ecc9c79f5a3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7d9ac1b-9394-44a5-9946-d834f286ff79");

            migrationBuilder.DropColumn(
                name: "CircuitId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IssueTypeId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "RequestorId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SLAExpiration",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Tickets",
                newName: "Requestor");

            migrationBuilder.AlterColumn<string>(
                name: "TicketRef",
                table: "Tickets",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SLA",
                table: "Circuits",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Companies_CompanyId",
                table: "Tickets",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
