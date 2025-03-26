using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASBNApp.DataAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkLocationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "WorkLocation",
                newName: "LocationName");

            migrationBuilder.RenameColumn(
                name: "Hours",
                table: "WorkLocation",
                newName: "SuggestedHours");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SuggestedHours",
                table: "WorkLocation",
                newName: "Hours");

            migrationBuilder.RenameColumn(
                name: "LocationName",
                table: "WorkLocation",
                newName: "Location");
        }
    }
}
