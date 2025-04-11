using System.ComponentModel.DataAnnotations.Schema;

namespace E_Ticket_System.Models
{
    public class ActorMovies
    {
        public int ActorId { get; set; }
        public int MovieId { get; set; }
        [ForeignKey("ActorId")]
        public Actor actor { get; set; }
        [ForeignKey("MovieId")]

        public Movie movie { get; set; }


    }
}
