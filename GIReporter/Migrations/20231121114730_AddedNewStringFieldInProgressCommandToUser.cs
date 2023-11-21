using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GIReporter.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewStringFieldInProgressCommandToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InProcessCommandName",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 183818121L,
                column: "InProcessCommandName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5097523800L,
                column: "InProcessCommandName",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InProcessCommandName",
                table: "Users");
        }
    }
}
