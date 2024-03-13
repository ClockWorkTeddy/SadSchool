using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SadSchool.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
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
                    value = table.Column<string>(type: "TEXT", nullable: true),
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
                    name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
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
                    grade = table.Column<int>(type: "INTEGER", nullable: true),
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
                    leader_id = table.Column<int>(type: "INTEGER", nullable: true),
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
                name: "scheduled_lessons",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    start_time_id = table.Column<int>(type: "INTEGER", nullable: true),
                    subject_id = table.Column<int>(type: "INTEGER", nullable: true),
                    class_id = table.Column<int>(type: "INTEGER", nullable: true),
                    teacher_id = table.Column<int>(type: "INTEGER", nullable: true),
                    day = table.Column<string>(type: "TEXT", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scheduled_lessons", x => x.id);
                    table.ForeignKey(
                        name: "FK_lesson_class",
                        column: x => x.class_id,
                        principalTable: "class",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_lesson_schedule",
                        column: x => x.start_time_id,
                        principalTable: "start_time",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_lesson_subject",
                        column: x => x.subject_id,
                        principalTable: "subject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_lesson_teacher",
                        column: x => x.teacher_id,
                        principalTable: "teacher",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    first_name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    last_name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
                    class_id = table.Column<int>(type: "INTEGER", nullable: true),
                    date_of_birth = table.Column<string>(type: "TEXT", nullable: true),
                    sex = table.Column<bool>(type: "INTEGER", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student", x => x.id);
                    table.ForeignKey(
                        name: "FK_student_class",
                        column: x => x.class_id,
                        principalTable: "class",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "lesson",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<string>(type: "TEXT", nullable: true),
                    scheduled_lesson_id = table.Column<int>(type: "INTEGER", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lesson", x => x.id);
                    table.ForeignKey(
                        name: "FK_mark_scheduled_lessons",
                        column: x => x.scheduled_lesson_id,
                        principalTable: "scheduled_lessons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "mark",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    value = table.Column<string>(type: "TEXT", nullable: false),
                    lesson_id = table.Column<int>(type: "INTEGER", nullable: true),
                    student_id = table.Column<int>(type: "INTEGER", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mark", x => x.id);
                    table.ForeignKey(
                        name: "FK_mark_lesson",
                        column: x => x.lesson_id,
                        principalTable: "lesson",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_mark_student",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_class_teacher_id",
                table: "class",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_scheduled_lesson_id",
                table: "lesson",
                column: "scheduled_lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_mark_lesson_id",
                table: "mark",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_mark_student_id",
                table: "mark",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_lessons_class_id",
                table: "scheduled_lessons",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_lessons_start_time_id",
                table: "scheduled_lessons",
                column: "start_time_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_lessons_subject_id",
                table: "scheduled_lessons",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_scheduled_lessons_teacher_id",
                table: "scheduled_lessons",
                column: "teacher_id");

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
                name: "scheduled_lessons");

            migrationBuilder.DropTable(
                name: "class");

            migrationBuilder.DropTable(
                name: "start_time");

            migrationBuilder.DropTable(
                name: "subject");

            migrationBuilder.DropTable(
                name: "teacher");
        }
    }
}
