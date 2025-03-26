using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASBNApp.DataAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntryModelPart2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldLocation",
                table: "LogEntry");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OldLocation",
                table: "LogEntry",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
