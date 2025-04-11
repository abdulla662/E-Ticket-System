using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Ticket_System.Migrations
{
    /// <inheritdoc />
    public partial class statusupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieStatus",
                table: "Movie");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MovieStatus",
                table: "Movie",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
