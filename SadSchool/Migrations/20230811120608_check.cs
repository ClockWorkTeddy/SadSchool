using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SadSchool.Migrations
{
    /// <inheritdoc />
    public partial class check : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "start_time",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_start_time", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subject",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "teacher",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    first_name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    last_name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    date_of_birth = table.Column<string>(type: "TEXT", nullable: true),
                    grade = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "class",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    teacher_id = table.Column<int>(type: "INTEGER", nullable: true),
                    leader_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class", x => x.id);
                    table.ForeignKey(
                        name: "FK_class_teacher",
                        column: x => x.teacher_id,
                        principalTable: "teacher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "lesson",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    start_time_id = table.Column<int>(type: "INTEGER", nullable: false),
                    subject_id = table.Column<int>(type: "INTEGER", nullable: false),
                    class_id = table.Column<int>(type: "INTEGER", nullable: false),
                    teacher_id = table.Column<int>(type: "INTEGER", nullable: false),
                    date = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lesson", x => x.id);
                    table.ForeignKey(
                        name: "FK_lesson_class",
                        column: x => x.class_id,
                        principalTable: "class",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_lesson_schedule_position",
                        column: x => x.start_time_id,
                        principalTable: "start_time",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_lesson_subject",
                        column: x => x.subject_id,
                        principalTable: "subject",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_lesson_teacher",
                        column: x => x.teacher_id,
                        principalTable: "teacher",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    first_name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    last_name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    class_id = table.Column<int>(type: "INTEGER", nullable: false),
                    date_of_birth = table.Column<string>(type: "TEXT", nullable: true),
                    sex = table.Column<bool>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student", x => x.id);
                    table.ForeignKey(
                        name: "FK_student_class",
                        column: x => x.class_id,
                        principalTable: "class",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "mark",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    value = table.Column<string>(type: "TEXT", nullable: false),
                    lesson_id = table.Column<int>(type: "INTEGER", nullable: true),
                    student_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mark", x => x.id);
                    table.ForeignKey(
                        name: "FK_mark_lesson",
                        column: x => x.lesson_id,
                        principalTable: "lesson",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_mark_student",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_class_teacher_id",
                table: "class",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_class_id",
                table: "lesson",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_start_time_id",
                table: "lesson",
                column: "start_time_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_subject_id",
                table: "lesson",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_teacher_id",
                table: "lesson",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_mark_lesson_id",
                table: "mark",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_mark_student_id",
                table: "mark",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_class_id",
                table: "student",
                column: "class_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mark");

            migrationBuilder.DropTable(
                name: "lesson");

            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "start_time");

            migrationBuilder.DropTable(
                name: "subject");

            migrationBuilder.DropTable(
                name: "class");

            migrationBuilder.DropTable(
                name: "teacher");
        }
    }
}
