using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASBNApp.DataAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntryModelPart1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "LogEntry",
                newName: "OldLocation");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "LogEntry",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogEntry_LocationId",
                table: "LogEntry",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogEntry_WorkLocation_LocationId",
                table: "LogEntry",
                column: "LocationId",
                principalTable: "WorkLocation",
                principalColumn: "Id");

            // Load sql script and execute
            using (var sr = new StreamReader("Migrations/20250218153319_UpdateEntryModelPart1.sql"))
            {
                migrationBuilder.Sql(sr.ReadToEnd());
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogEntry_WorkLocation_LocationId",
                table: "LogEntry");

            migrationBuilder.DropIndex(
                name: "IX_LogEntry_LocationId",
                table: "LogEntry");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "LogEntry");

            migrationBuilder.RenameColumn(
                name: "OldLocation",
                table: "LogEntry",
                newName: "Location");
        }
    }
}
