using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GIReporter.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserStateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTypeProjectName",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserState",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 183818121L,
                column: "UserState",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserState",
                table: "Users");

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
    }
}
