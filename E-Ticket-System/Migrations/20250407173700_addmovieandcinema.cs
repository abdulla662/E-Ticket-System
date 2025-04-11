using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Ticket_System.Migrations
{
    /// <inheritdoc />
    public partial class addmovieandcinema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "pendingTickets",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_pendingTickets_CinemaId",
                table: "pendingTickets",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_pendingTickets_MovieId",
                table: "pendingTickets",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_pendingTickets_UserId",
                table: "pendingTickets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_pendingTickets_AspNetUsers_UserId",
                table: "pendingTickets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_pendingTickets_Cinema_CinemaId",
                table: "pendingTickets",
                column: "CinemaId",
                principalTable: "Cinema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_pendingTickets_Movie_MovieId",
                table: "pendingTickets",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pendingTickets_AspNetUsers_UserId",
                table: "pendingTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_pendingTickets_Cinema_CinemaId",
                table: "pendingTickets");

            migrationBuilder.DropForeignKey(
                name: "FK_pendingTickets_Movie_MovieId",
                table: "pendingTickets");

            migrationBuilder.DropIndex(
                name: "IX_pendingTickets_CinemaId",
                table: "pendingTickets");

            migrationBuilder.DropIndex(
                name: "IX_pendingTickets_MovieId",
                table: "pendingTickets");

            migrationBuilder.DropIndex(
                name: "IX_pendingTickets_UserId",
                table: "pendingTickets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "pendingTickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
