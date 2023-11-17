using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GIReporter.Migrations
{
    /// <inheritdoc />
    public partial class AddedBahodirAkaUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "UserState" },
                values: new object[] { 5097523800L, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5097523800L);
        }
    }
}
