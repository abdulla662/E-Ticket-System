using E_Ticket_System.Models;
using E_Ticket_System.Repositries;
using E_Ticket_System.Repositries.Irepostries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace E_Ticket_System.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]



    public class HomeController : Controller
    { 
        private readonly ImovieRepository _movieRepository;
        public HomeController(ImovieRepository movieRepository)
        {
            this._movieRepository = movieRepository;
        }


        private readonly ILogger<HomeController> _logger;

     

        public IActionResult Index()
        {
            var data = _movieRepository.Get(includes: [e => e.Cinema]);
            if (data != null) {
                return View(data.ToList());

            }
            return  RedirectToAction("ErrorPage");  
        }
        public IActionResult ErrorPage() { 
        return View();
        }
        public IActionResult search(string keyword)
        {
            var data = _movieRepository.Get(e=> e.Name.Contains(keyword), includes:[e=> e.Cinema]).ToList();
            
                return View("Index", data.ToList());



        }



    }
}
