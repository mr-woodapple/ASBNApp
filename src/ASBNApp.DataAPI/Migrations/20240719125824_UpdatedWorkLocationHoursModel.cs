using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASBNApp.DataAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedWorkLocationHoursModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "WorkLocation",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "WorkLocation",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLocation_OwnerId",
                table: "WorkLocation",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocation_AspNetUsers_OwnerId",
                table: "WorkLocation",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocation_AspNetUsers_OwnerId",
                table: "WorkLocation");

            migrationBuilder.DropIndex(
                name: "IX_WorkLocation_OwnerId",
                table: "WorkLocation");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "WorkLocation");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WorkLocation",
                newName: "ID");
        }
    }
}
