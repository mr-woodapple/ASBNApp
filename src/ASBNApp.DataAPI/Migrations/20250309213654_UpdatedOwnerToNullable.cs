using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASBNApp.DataAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedOwnerToNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocation_AspNetUsers_OwnerId",
                table: "WorkLocation");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "WorkLocation",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocation_AspNetUsers_OwnerId",
                table: "WorkLocation",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkLocation_AspNetUsers_OwnerId",
                table: "WorkLocation");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "WorkLocation",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLocation_AspNetUsers_OwnerId",
                table: "WorkLocation",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
