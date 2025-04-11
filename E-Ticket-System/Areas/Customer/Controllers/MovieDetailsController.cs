using E_Ticket_System.Models;
using E_Ticket_System.Repositries;
using E_Ticket_System.Repositries.Irepostries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace E_Ticket_System.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]

    public class MovieDetailsController : Controller
    {
        private readonly ImovieRepository movieRepository;
        private readonly IActorRepository actorRepository;
        public MovieDetailsController(ImovieRepository movie, IActorRepository actorRepository)
        {
            this.movieRepository = movie;
            this.actorRepository = actorRepository;
        }

            

            public IActionResult Details(int movieid)
            {
                var data = movieRepository.GetOne(
                    x => x.Id == movieid,
                    includes: new Expression<Func<Movie, object>>[]
                    {
                    e => e.ActorMovies,
                    e => e.Category,
                    e => e.Cinema
                    });

                if (data == null)
                {
                    return Redirect("/Customer/Home/ErrorPage");
                }

                var actors = actorRepository.Get(
                    e => e.ActorMovies.Any(am => am.MovieId == movieid),
                    new Expression<Func<Actor, object>>[] { e => e.ActorMovies }
                );

                ViewBag.Actors = actors.ToList();
                return View(data);
            }
            public IActionResult ActorDetails(int actorid) {

                var data = actorRepository.GetOne(e => e.Id == actorid);
                if (data == null)
                {
                    return Redirect("/Customer/Home/ErrorPage");

                }
                ViewBag.Movies = movieRepository.Get(m => m.ActorMovies.Any(am => am.ActorId == actorid)).ToList();

                return View(data);
            }
        }
    }
