using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SadSchool.Migrations
{
    /// <inheritdoc />
    public partial class mark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "count",
                table: "class");

            migrationBuilder.DropColumn(
                name: "students_quantity",
                table: "class");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "lesson",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "mark",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    value = table.Column<string>(type: "TEXT", nullable: false),
                    lesson_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mark", x => x.id);
                    table.ForeignKey(
                        name: "FK_mark_lesson",
                        column: x => x.lesson_id,
                        principalTable: "lesson",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_mark_lesson_id",
                table: "mark",
                column: "lesson_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mark");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "lesson");

            migrationBuilder.AddColumn<int>(
                name: "count",
                table: "class",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "students_quantity",
                table: "class",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
