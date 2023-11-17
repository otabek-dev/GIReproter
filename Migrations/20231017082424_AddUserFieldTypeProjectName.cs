using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GIReporter.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFieldTypeProjectName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTypeProjectName",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 183818121L,
                column: "IsTypeProjectName",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTypeProjectName",
                table: "Users");
        }
    }
}
