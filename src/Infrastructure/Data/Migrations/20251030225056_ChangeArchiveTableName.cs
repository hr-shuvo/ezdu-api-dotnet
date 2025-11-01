using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeArchiveTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_ExamArchive_ExamArchiveId",
                table: "Question");

            migrationBuilder.DropTable(
                name: "ExamArchive");

            migrationBuilder.RenameColumn(
                name: "ExamArchiveId",
                table: "Question",
                newName: "ArchiveExamId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_ExamArchiveId",
                table: "Question",
                newName: "IX_Question_ArchiveExamId");

            migrationBuilder.CreateTable(
                name: "ArchiveExam",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ClassId = table.Column<long>(type: "bigint", nullable: false),
                    SubjectId = table.Column<long>(type: "bigint", nullable: false),
                    InstituteId = table.Column<long>(type: "bigint", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchiveExam", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ArchiveExam_InstituteId",
                table: "ArchiveExam",
                column: "InstituteId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchiveExam_SubjectId",
                table: "ArchiveExam",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_ArchiveExam_ArchiveExamId",
                table: "Question",
                column: "ArchiveExamId",
                principalTable: "ArchiveExam",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_ArchiveExam_ArchiveExamId",
                table: "Question");

            migrationBuilder.DropTable(
                name: "ArchiveExam");

            migrationBuilder.RenameColumn(
                name: "ArchiveExamId",
                table: "Question",
                newName: "ExamArchiveId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_ArchiveExamId",
                table: "Question",
                newName: "IX_Question_ExamArchiveId");

            migrationBuilder.CreateTable(
                name: "ExamArchive",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ClassId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    InstituteId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamArchive", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ExamArchive_InstituteId",
                table: "ExamArchive",
                column: "InstituteId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamArchive_SubjectId",
                table: "ExamArchive",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_ExamArchive_ExamArchiveId",
                table: "Question",
                column: "ExamArchiveId",
                principalTable: "ExamArchive",
                principalColumn: "Id");
        }
    }
}
