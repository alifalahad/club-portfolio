using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHorizon.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToTeamMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TeamMembers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "TeamMembers");
        }
    }
}
