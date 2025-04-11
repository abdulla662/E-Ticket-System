namespace E_Ticket_System.Models.ViewModel
{
    public class MovieWithDetailVM
    {
        public Movie? Movie { get; set; }
        public List<Cinema>? Cinemas { get; set; }
        public List<Actor>? Actors { get; set; }

        public List<Category>? Categories { get; set; }

        public List<int>? SelectedActors { get; set; } 



    }
}
