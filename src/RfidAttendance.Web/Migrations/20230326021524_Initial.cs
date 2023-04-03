using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RfidAttendance.Web.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rfid_tags",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    is_currently_in_attendance = table.Column<bool>(type: "INTEGER", nullable: false),
                    last_seen = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rfid_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    first_name = table.Column<string>(type: "TEXT", nullable: false),
                    last_name = table.Column<string>(type: "TEXT", nullable: false),
                    rfid_tag_id = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.id);
                    table.ForeignKey(
                        name: "FK_students_rfid_tags_rfid_tag_id",
                        column: x => x.rfid_tag_id,
                        principalTable: "rfid_tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_students_rfid_tag_id",
                table: "students",
                column: "rfid_tag_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "rfid_tags");
        }
    }
}
