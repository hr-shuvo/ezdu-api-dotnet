using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuestionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ExamArchiveId",
                table: "Question",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Question_ExamArchiveId",
                table: "Question",
                column: "ExamArchiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_ExamArchive_ExamArchiveId",
                table: "Question",
                column: "ExamArchiveId",
                principalTable: "ExamArchive",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_ExamArchive_ExamArchiveId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_ExamArchiveId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ExamArchiveId",
                table: "Question");
        }
    }
}
