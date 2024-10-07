using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASBNApp.DataAPI.Migrations
{
    /// <inheritdoc />
    public partial class renamedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entry_AspNetUsers_OwnerId",
                table: "Entry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entry",
                table: "Entry");

            migrationBuilder.RenameTable(
                name: "Entry",
                newName: "LogEntry");

            migrationBuilder.RenameIndex(
                name: "IX_Entry_OwnerId",
                table: "LogEntry",
                newName: "IX_LogEntry_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogEntry",
                table: "LogEntry",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LogEntry_AspNetUsers_OwnerId",
                table: "LogEntry",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogEntry_AspNetUsers_OwnerId",
                table: "LogEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogEntry",
                table: "LogEntry");

            migrationBuilder.RenameTable(
                name: "LogEntry",
                newName: "Entry");

            migrationBuilder.RenameIndex(
                name: "IX_LogEntry_OwnerId",
                table: "Entry",
                newName: "IX_Entry_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entry",
                table: "Entry",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entry_AspNetUsers_OwnerId",
                table: "Entry",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
