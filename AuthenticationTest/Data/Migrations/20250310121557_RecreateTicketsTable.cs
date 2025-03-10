using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace AuthenticationTest.Data.Migrations
{
    public partial class RecreateTicketsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ ✅ Drop foreign key constraints
            

            // 3️⃣ ✅ Create the new table
            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketRef = table.Column<string>(nullable: false),
                    AssigneeId = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    CircuitId = table.Column<int>(nullable: true),
                    IssueTypeId = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    ProviderRef = table.Column<string>(nullable: true),
                    CC = table.Column<string>(nullable: false, defaultValue: "N/A"),
                    InternalNotesHistory = table.Column<string>(nullable: false, defaultValue: "N/A"),
                    CustomerCommunicationHistory = table.Column<string>(nullable: false, defaultValue: "N/A"),
                    RequestorEmail = table.Column<string>(nullable: false, defaultValue: "N/A"),
                    SiteName = table.Column<string>(nullable: true),
                    VLAN = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Companies",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Circuits",
                        column: x => x.CircuitId,
                        principalTable: "Circuits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_IssueTypes",
                        column: x => x.IssueTypeId,
                        principalTable: "IssueTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Departments",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // ✅ Recreate indexes
            migrationBuilder.CreateIndex(name: "IX_Tickets_AssigneeId", table: "Tickets", column: "AssigneeId");
            migrationBuilder.CreateIndex(name: "IX_Tickets_CompanyId", table: "Tickets", column: "CompanyId");
            migrationBuilder.CreateIndex(name: "IX_Tickets_CircuitId", table: "Tickets", column: "CircuitId");
            migrationBuilder.CreateIndex(name: "IX_Tickets_IssueTypeId", table: "Tickets", column: "IssueTypeId");
            migrationBuilder.CreateIndex(name: "IX_Tickets_DepartmentId", table: "Tickets", column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ✅ Drop the table if rolling back
            migrationBuilder.DropTable(name: "Tickets");
        }
    }
}
