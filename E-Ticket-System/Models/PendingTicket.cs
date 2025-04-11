using System.ComponentModel.DataAnnotations.Schema;

namespace E_Ticket_System.Models
{
    public class PendingTicket
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; }
        public Movie Movie { get; set; }
        public Cinema Cinema { get; set; }
        public ApplicationUser User { get; set; }   

        public int MovieId { get; set; }
        public int CinemaId { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsProcessed { get; set; } = false;
        public string? SessionId { get; set; }

        public string? PayementStripId { get; set; }
    }
}
