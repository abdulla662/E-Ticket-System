using E_Ticket_System.Models;
using E_Ticket_System.Repositries;
using E_Ticket_System.Repositries.Irepostries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Ticket_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CinemaController : Controller
    {
        private readonly IcinemaReposirotry cinemaReposirotry;
        public CinemaController(IcinemaReposirotry cinemaReposirotry)
        {
            this.cinemaReposirotry = cinemaReposirotry;
        }
        public IActionResult Index()
        {
            var data = cinemaReposirotry.Get();
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cinema cinema) {
            if (cinema != null && ModelState.IsValid)
            {
                cinemaReposirotry.Create(cinema);
                
                cinemaReposirotry.comit();
                return RedirectToAction(nameof(Index)); 
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Cinema = cinemaReposirotry.GetOne(e => e.Id == id);
            if (Cinema == null)
            {
                return NotFound();
            }
            return View(Cinema);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Cinema cinema) {
            if (cinema != null && ModelState.IsValid) { 
                var data=cinemaReposirotry.GetOne(e=> e.Id==cinema.Id);
                data.Name=cinema.Name;
                cinemaReposirotry.Edit(data);
                cinemaReposirotry.comit(); 
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            var data = cinemaReposirotry.GetOne(e => e.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            cinemaReposirotry.Delete(data);
            cinemaReposirotry.comit();
            return RedirectToAction(nameof(Index));
        }

    }
}

