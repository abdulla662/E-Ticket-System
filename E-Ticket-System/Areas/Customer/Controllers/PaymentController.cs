using E_Ticket_System.Models;
using E_Ticket_System.Repositries.Irepostries;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Stripe.Checkout;
using Stripe;
using E_Ticket_System.Repositries;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace E_Ticket_System.Areas.Customer.Controllers
{

    [Area("Customer")]
    [Authorize]

    public class PaymentController : Controller
    {
        private readonly ImovieRepository _imovieRepository;
        private readonly IPendingTicketRepository _pendingticketRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public PaymentController(ImovieRepository movieRepository,  UserManager<ApplicationUser> userManager,  IPendingTicketRepository IpendingticketRepository)
        {
            _imovieRepository = movieRepository;
            _userManager = userManager;
            _pendingticketRepository = IpendingticketRepository;
        }
        public IActionResult Index(int id)
        {
            var data = _imovieRepository.GetOne(e => e.Id == id, includes: [e => e.Cinema]);
            return View(data);
        }
        public IActionResult chooseseat(int movieid)
        {

            var movie = _imovieRepository.GetOne(e => e.Id == movieid, includes: [e => e.Cinema]);

            if (movie == null)
            {
                return NotFound();
            }
            var bookedSeats = _pendingticketRepository
       .Get(t => t.MovieId == movieid && t.IsProcessed==true)
       .Select(t => t.SeatNumber)
       .ToList();

            ViewBag.BookedSeats = bookedSeats; 

            return View(movie);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReserveSeats(string selectedSeats, int movieId, int cinemaId)
        {

            var userid = _userManager.GetUserId(User);
            var seats = selectedSeats.Split(',');
            var movie = movieId;
            var cinema = cinemaId;
            var oldPendingTickets = _pendingticketRepository.Get(p => p.UserId == userid && !p.IsProcessed).ToList();
            foreach (var ticket in oldPendingTickets)
            {
                _pendingticketRepository.Delete(ticket);
            }

            foreach (var seatNumber in seats)
            {
                var PendingTicket = new PendingTicket
                {

                    SeatNumber = seatNumber,
                    MovieId = movie,
                    CinemaId = cinema,
                    UserId = userid
                };

                _pendingticketRepository.Create(PendingTicket);
               
            }
            _pendingticketRepository.comit();       
            return RedirectToAction(nameof(pay));
        }
        public IActionResult pay()
        {
            var userId = _userManager.GetUserId(User);


            var pendingTickets = _pendingticketRepository.Get(e => e.UserId == userId && !e.IsProcessed, includes: [e=> e.Movie, p=>p.Cinema]).ToList();
            var ticketid = _pendingticketRepository.GetOne(e => e.UserId == userId && !e.IsProcessed, includes: [e => e.Movie, p => p.Cinema]);
            var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Sucess?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/cancel",       
            };


            foreach (var item in pendingTickets) {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "EGP",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Movie.Name,
                            Description = item.Movie.Description,
                        },
                        UnitAmount = (long)(item.Movie.Price * 100), 
                    },
                    Quantity = 1,
                });
            }
          
            var service = new SessionService();
            var session = service.Create(options);
            foreach (var ticket in pendingTickets)
            {
                ticket.SessionId = session.Id;
                ticket.PayementStripId = session.PaymentIntentId;
                _pendingticketRepository.Edit(ticket);
            }

            _pendingticketRepository.comit();

            return Redirect(session.Url);
        }
    }
    }
