using E_Ticket_System.Models;
using E_Ticket_System.Repositries;
using E_Ticket_System.Repositries.Irepostries;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;
using Stripe;
using Stripe.Checkout;
using E_Ticket_System.Utility;
using QuestPDF.Helpers;
using QuestPDF.Fluent;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace E_Ticket_System.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]

    public class myticketController : Controller
    {
        private readonly IPendingTicketRepository _pendingTicketRepossitory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _env;

        public myticketController(IPendingTicketRepository pendingTicketRepossitory, UserManager<ApplicationUser> userManager, IEmailSender emailsender, IWebHostEnvironment env)
        {
            this._pendingTicketRepossitory = pendingTicketRepossitory;
            this._userManager = userManager;
            this._emailSender = emailsender;
            _env = env;

        }

        public IActionResult Index()
        {
            var UserId = _userManager.GetUserId(User);
            var data = _pendingTicketRepossitory.Get(e => e.UserId == UserId, includes: [e => e.Movie, e => e.Cinema]);
            if (UserId != null)
            {
                return View(data);

            }
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = _userManager.GetUserId(User);
            var targetTicket = _pendingTicketRepossitory.GetOne(
                e => e.Id == id && e.UserId == userId, includes: [e => e.Movie, e => e.Cinema, e => e.User]
            );
            if (targetTicket == null)
            {
                TempData["Error"] = "Ticket not found.";
                return RedirectToAction(nameof(Index));
            }

            if ((targetTicket.Movie.StartDate.ToUniversalTime() == DateTime.Now))
            {
                TempData["Error"] = "You cannot cancel the ticket in the same day of movie";
                return RedirectToAction(nameof(Index));
            }

            var relatedTickets = _pendingTicketRepossitory.Get(
                e => e.MovieId == targetTicket.MovieId &&
                     e.UserId == userId &&
                     e.PayementStripId == targetTicket.PayementStripId,
                includes: [e => e.Movie, e => e.Cinema, e => e.User]
            ).ToList();

            var totalAmount = relatedTickets.Sum(t => t.Movie.Price);
            var refundOptions = new RefundCreateOptions
            {
                PaymentIntent = targetTicket.PayementStripId,
                Amount = (long)totalAmount,
                Reason = RefundReasons.RequestedByCustomer,
            };
            var refundService = new RefundService();
            var refundSession = refundService.Create(refundOptions);

            foreach (var ticket in relatedTickets)
            {
                _pendingTicketRepossitory.Delete(ticket);
            }
            _pendingTicketRepossitory.comit();

            string subject = "Ticket Cancelation";
            string message = $"Dear Customer, your {targetTicket.Id}  for movie '{targetTicket.Movie.Name}' have been cancelled and refunded.";
            if (targetTicket.User?.Email != null)
            {
                await _emailSender.SendEmailAsync(targetTicket.User.Email, subject, message);
            }

            TempData["Success"] = "Tickets cancelled successfully.";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult DownloadTicket(int id)
        {
            var userId = _userManager.GetUserId(User);
            var targetTicket = _pendingTicketRepossitory.GetOne(
                e => e.Id == id && e.UserId == userId,
                includes: [e => e.Movie, e => e.Cinema, e => e.User]
            );

            if (targetTicket == null)
                return NotFound();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // QuestPDF Community License requirement
                    page.Footer().AlignCenter().Text("QuestPDF Community License");

                    page.Content()
                        .PaddingVertical(20)
                        .Column(col =>
                        {
                            // Header Section
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().AlignLeft().Text("E-Ticket System").Bold().FontSize(18);
                                row.RelativeItem().AlignRight().Text($"Ticket #{targetTicket.Id}").SemiBold();
                            });

                            // Ticket Details Section
                            col.Item().PaddingTop(10).Row(row =>
                            {
                                row.RelativeItem().Column(details =>
                                {
                                    details.Item().Text(targetTicket.Movie.Name).Bold().FontSize(24);
                                    details.Item().Text($"Cinema: {targetTicket.Cinema.Name}").FontSize(16);
                                    details.Item().Text($"Time: {targetTicket.Movie.StartDate:dd MMM yyyy, hh:mm tt}").FontSize(16);
                                    details.Item().Text($"Seats: {string.Join(", ", targetTicket.SeatNumber)}").FontSize(16);
                                });
                            });

                            // Terms & Conditions Footer
                            col.Item().PaddingTop(30).Background(Colors.Grey.Lighten3).Padding(10).Text(text =>
                            {
                                text.Span("Terms & Conditions: ").Bold();
                                text.Span("This ticket is non-refundable. Arrive 30 mins before showtime.");
                            });

                            // Stamp Section
                            string stampImagePath = Path.Combine(_env.WebRootPath, "images", "Stamp", "stamp.png");
                            bool stampExists = System.IO.File.Exists(stampImagePath);

                            col.Item()
                                .PaddingTop(20)
                                .ExtendVertical() // Forces content to bottom
                                .AlignCenter()
                                .Element(e =>
                                {
                                    if (stampExists)
                                    {
                                        try
                                        {
                                            e.Width(300)
                                             .Height(200)
                                             .Image(stampImagePath);
                                        }
                                        catch
                                        {
                                            e.Text("INVALID STAMP IMAGE")
                                             .FontColor(Colors.Red.Medium)
                                             .Bold();
                                        }
                                    }
                                    else
                                    {
                                        e.Text("OFFICIAL TICKET")
                                         .FontColor(Colors.Red.Medium)
                                         .Bold();
                                    }
                                });
                        });
                });
            });

            byte[] pdfBytes = document.GeneratePdf();
            return File(pdfBytes, "application/pdf", $"Ticket_{targetTicket.Id}.pdf");
        }


    }
}
