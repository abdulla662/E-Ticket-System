using System.ComponentModel.DataAnnotations;

namespace E_Ticket_System.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3,ErrorMessage ="please enter more than 3 char")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }

        public string? ImgUrl { get; set; }

        public string TrailerUrl { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }


        [Required(ErrorMessage = "The Cinema field is required.")]
        public int CinemaId { get; set; }

        public Cinema? Cinema { get; set; }

        [Required(ErrorMessage = "The Category field is required.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<ActorMovies> ActorMovies { get; set; } = new List<ActorMovies>();


        public bool? MovieStatus
        {
            get
            {
                if (DateTime.Now >= StartDate && DateTime.Now <= EndDate)
                {
                    return true; 
                }
                else
                {
                    return false; 
                }
            }
        }
    }
}

