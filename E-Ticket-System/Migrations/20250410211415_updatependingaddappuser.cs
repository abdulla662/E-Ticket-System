using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Ticket_System.Migrations
{
    /// <inheritdoc />
    public partial class updatependingaddappuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userTickets");

            migrationBuilder.DropTable(
                name: "tikcets");

            migrationBuilder.AddColumn<string>(
                name: "applicationUserId",
                table: "pendingTickets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_pendingTickets_applicationUserId",
                table: "pendingTickets",
                column: "applicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_pendingTickets_AspNetUsers_applicationUserId",
                table: "pendingTickets",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pendingTickets_AspNetUsers_applicationUserId",
                table: "pendingTickets");

            migrationBuilder.DropIndex(
                name: "IX_pendingTickets_applicationUserId",
                table: "pendingTickets");

            migrationBuilder.DropColumn(
                name: "applicationUserId",
                table: "pendingTickets");

            migrationBuilder.CreateTable(
                name: "tikcets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CinemaId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    IsBooked = table.Column<bool>(type: "bit", nullable: false),
                    PaymentStripeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeatNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tikcets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tikcets_Cinema_CinemaId",
                        column: x => x.CinemaId,
                        principalTable: "Cinema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tikcets_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "userTickets",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userTickets", x => new { x.UserId, x.TicketId });
                    table.ForeignKey(
                        name: "FK_userTickets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userTickets_tikcets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "tikcets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tikcets_CinemaId",
                table: "tikcets",
                column: "CinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_tikcets_MovieId",
                table: "tikcets",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_userTickets_TicketId",
                table: "userTickets",
                column: "TicketId");
        }
    }
}
