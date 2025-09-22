using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTitleToName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TopicContent",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Topic",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Subject",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Quiz",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Question",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Lesson",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Class",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Assignment",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Groups",
                table: "Class",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AuthToken",
                type: "longtext",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Achievement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievement", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Achievement");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AuthToken");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TopicContent",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Topic",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Subject",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Quiz",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Question",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Lesson",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Class",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Assignment",
                newName: "Title");

            migrationBuilder.AlterColumn<string>(
                name: "Groups",
                table: "Class",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
