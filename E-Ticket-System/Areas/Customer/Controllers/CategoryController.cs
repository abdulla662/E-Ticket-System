using E_Ticket_System.DataAcess;
using E_Ticket_System.Models;
using E_Ticket_System.Repositries;
using E_Ticket_System.Repositries.Irepostries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Ticket_System.Areas.Customer.Controllers
{
    [Area("Customer")]
[Authorize]
    public class CategoryController : Controller
    {
        //CategoryRepository categoryRepository = new CategoryRepository();
        //CinemaRepository cinemaRepository = new CinemaRepository();
        private readonly ICategoryRepository categoryRepository;
        private readonly IcinemaReposirotry CinemaRepository;
        public CategoryController(ICategoryRepository categoryRepository,IcinemaReposirotry CinemaRepository)
        {
            this.categoryRepository = categoryRepository;
            this.CinemaRepository = CinemaRepository;
        }


        public IActionResult CategoryView()
        {
            var data = categoryRepository.Get();
            return View(data.ToList());
        }
        public IActionResult CategoryMovie(int id)
        {
            var category = categoryRepository.GetOne(e => e.Id == id, includes: [e => e.Movies]);

            if (category == null)
            {
                return Redirect("/Customer/Home/ErrorPage");
            }

            var cinemaIds = category.Movies.Select(m => m.CinemaId).Distinct();
            var cinemas = CinemaRepository.Get(c => cinemaIds.Contains(c.Id)); 

            ViewBag.Cinemas = cinemas.ToList();

            return View(category);
        }


    }
    }