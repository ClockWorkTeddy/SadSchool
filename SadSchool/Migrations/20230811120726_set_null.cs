using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SadSchool.Migrations
{
    /// <inheritdoc />
    public partial class set_null : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_class",
                table: "lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_schedule_position",
                table: "lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_subject",
                table: "lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_teacher",
                table: "lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_mark_lesson",
                table: "mark");

            migrationBuilder.DropForeignKey(
                name: "FK_mark_student",
                table: "mark");

            migrationBuilder.DropForeignKey(
                name: "FK_student_class",
                table: "student");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_class",
                table: "lesson",
                column: "class_id",
                principalTable: "class",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_schedule_position",
                table: "lesson",
                column: "start_time_id",
                principalTable: "start_time",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_subject",
                table: "lesson",
                column: "subject_id",
                principalTable: "subject",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_teacher",
                table: "lesson",
                column: "teacher_id",
                principalTable: "teacher",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_mark_lesson",
                table: "mark",
                column: "lesson_id",
                principalTable: "lesson",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_mark_student",
                table: "mark",
                column: "student_id",
                principalTable: "student",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_student_class",
                table: "student",
                column: "class_id",
                principalTable: "class",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_class",
                table: "lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_schedule_position",
                table: "lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_subject",
                table: "lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_teacher",
                table: "lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_mark_lesson",
                table: "mark");

            migrationBuilder.DropForeignKey(
                name: "FK_mark_student",
                table: "mark");

            migrationBuilder.DropForeignKey(
                name: "FK_student_class",
                table: "student");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_class",
                table: "lesson",
                column: "class_id",
                principalTable: "class",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_schedule_position",
                table: "lesson",
                column: "start_time_id",
                principalTable: "start_time",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_subject",
                table: "lesson",
                column: "subject_id",
                principalTable: "subject",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_teacher",
                table: "lesson",
                column: "teacher_id",
                principalTable: "teacher",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_mark_lesson",
                table: "mark",
                column: "lesson_id",
                principalTable: "lesson",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_mark_student",
                table: "mark",
                column: "student_id",
                principalTable: "student",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_student_class",
                table: "student",
                column: "class_id",
                principalTable: "class",
                principalColumn: "id");
        }
    }
}
