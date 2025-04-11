using E_Ticket_System.Repositries;
using E_Ticket_System.Repositries.Irepostries;
using E_Ticket_System.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe.Climate;

namespace E_Ticket_System.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]

    public class Checkout : Controller
    {
        private readonly IPendingTicketRepository _pendingticketRepository;
        private readonly IEmailSender _emailSender;


        public Checkout(IPendingTicketRepository pendingticketRepository, IEmailSender emailSender)
        {
            _pendingticketRepository = pendingticketRepository;
            _emailSender = emailSender;
        }
        public async Task <IActionResult> Sucess(string session_id)
        {

            if (!string.IsNullOrEmpty(session_id))
            {
                var pendingTickets = _pendingticketRepository.Get(
                     e => e.SessionId == session_id && !e.IsProcessed,
                     includes: [e => e.Movie, p => p.Cinema, p => p.User]
                 ).ToList();

                var service = new SessionService();
                var session = service.Get(session_id);

                foreach (var ticket in pendingTickets)
                {
                    ticket.IsProcessed = true;
                    ticket.CreatedAt = DateTime.Now;
                    ticket.PayementStripId = session.PaymentIntentId;
                    _pendingticketRepository.Edit(ticket);
                }

                _pendingticketRepository.comit();
                var firstTicket = pendingTickets.FirstOrDefault();
                if (firstTicket?.User?.Email != null)
                {
                    string subject = "Ticket Confirmation";
                    string message = $"Dear Customer,\n\n" +
                                     $"You have successfully booked {pendingTickets.Count} ticket(s) for the movie \"{firstTicket.Movie.Name}\" at {firstTicket.Cinema.Name}.\n" +
                                     $"Enjoy your show!\n\n" +
                                     $"Thank you for booking with us.";
                   await _emailSender.SendEmailAsync(firstTicket.User.Email, subject, message);

                }

            }
            return RedirectToAction("Index", "Home", new { area = "Customer" });

        }
    }
}
