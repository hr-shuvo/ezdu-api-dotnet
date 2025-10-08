using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTopicContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LessonId",
                table: "TopicContent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SubjectId",
                table: "TopicContent",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_TopicContent_LessonId",
                table: "TopicContent",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicContent_SubjectId",
                table: "TopicContent",
                column: "SubjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TopicContent_LessonId",
                table: "TopicContent");

            migrationBuilder.DropIndex(
                name: "IX_TopicContent_SubjectId",
                table: "TopicContent");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "TopicContent");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "TopicContent");
        }
    }
}
