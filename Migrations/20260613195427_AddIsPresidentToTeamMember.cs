using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventHorizon.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPresidentToTeamMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPresident",
                table: "TeamMembers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPresident",
                table: "TeamMembers");
        }
    }
}
