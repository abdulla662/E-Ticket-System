using E_Ticket_System.Repositries;
using E_Ticket_System.Repositries.Irepostries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticket_System.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]

    public class CinemaController : Controller
    {
        private readonly IcinemaReposirotry cinemaRepository;
        public CinemaController(IcinemaReposirotry cinemarepository)
        {
            this.cinemaRepository = cinemarepository;
        }

        public IActionResult CinemaView ()
        {
            var data = cinemaRepository.Get();
            return View(data);
        }
        public IActionResult CinemaMovie(int id)
        {
            var cinema = cinemaRepository.GetOne(e => e.Id == id, includes: [e => e.Movies]);

            if (cinema == null)
            {
                return Redirect("/Customer/Home/ErrorPage");
            }

            return View(cinema);
        }

    }
}
