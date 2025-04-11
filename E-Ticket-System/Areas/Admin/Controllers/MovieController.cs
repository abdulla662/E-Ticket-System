using E_Ticket_System.Models;
using E_Ticket_System.Models.ViewModel;
using E_Ticket_System.Repositries;
using E_Ticket_System.Repositries.Irepostries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace E_Ticket_System.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class MovieController : Controller
    {
        private readonly ImovieRepository movieRepository;
        private readonly IActorRepository actorRepository;
        private readonly IcinemaReposirotry cinemaRepository;
        private readonly ICategoryRepository categoryRepository;

        public MovieController(ImovieRepository movieRepository, IActorRepository actorRepository, IcinemaReposirotry cinemaRepository, ICategoryRepository categoryRepository)
        {
            this.movieRepository = movieRepository;
            this.actorRepository = actorRepository;
            this.cinemaRepository = cinemaRepository;
            this.categoryRepository = categoryRepository;
        }
        [HttpGet]
        public IActionResult index()
        {
            var data = movieRepository.Get();
            return View(model:data.ToList());
        }

        [HttpGet]
        public IActionResult create()
        {
            var model = new MovieWithDetailVM()
            {
                Cinemas = cinemaRepository.Get().ToList(),
                Actors = actorRepository.Get().ToList(),
                Categories = categoryRepository.Get().ToList()


            };
            return View(model);

        }
        [HttpPost]
        public IActionResult create(MovieWithDetailVM model)
        {
            if (!ModelState.IsValid)
            {

                model.Cinemas = cinemaRepository.Get().ToList();
                model.Actors = actorRepository.Get().ToList();
                model.Categories = categoryRepository.Get().ToList();
                return View(model);
            }
            var movie = new Movie
            {
                Name = model.Movie.Name,
                Description = model.Movie.Description,
                Price = model.Movie.Price,
                ImgUrl = model.Movie.ImgUrl,
                TrailerUrl = model.Movie.TrailerUrl,
                StartDate = model.Movie.StartDate,
                EndDate = model.Movie.EndDate,
                CinemaId = model.Movie.CinemaId,
                CategoryId = model.Movie.CategoryId,
            };

            if (model.SelectedActors != null)
            {
                foreach (var actorId in model.SelectedActors)
                {
                    movie.ActorMovies.Add(new ActorMovies { ActorId = actorId });
                }
            }

            movieRepository.Create(movie);
            return RedirectToAction("Index");

        }
        public IActionResult Edit(int id)
        {
            var data = movieRepository.GetOne(
         e => e.Id == id,
         includes: new Expression<Func<Movie, object>>[]
         {
            e => e.Cinema,
            e => e.Category,
            e => e.ActorMovies
         });

            if (data == null)
            {
                return NotFound();
            }

            ViewBag.Cinema = new SelectList(cinemaRepository.Get(), "Id", "Name", data.CinemaId);
            ViewBag.Category = new SelectList(categoryRepository.Get(), "Id", "Name", data.CategoryId);
            ViewBag.Actors = new MultiSelectList(actorRepository.Get(), "Id", "FullName", data.ActorMovies.Select(a => a.ActorId));

            return View(data);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Movie model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Cinema = cinemaRepository.Get();
                ViewBag.Category = categoryRepository.Get();
                return View(model);
            }

            movieRepository.Edit(model);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int movieid)
        {
            var data = movieRepository.GetOne(e => e.Id == movieid);

            if (data == null)
            {
                return NotFound(); // Return 404 if the movie does not exist
            }

            movieRepository.Delete(data);

            movieRepository.comit(); 

            return RedirectToAction("Index"); 
        }

    }
}




