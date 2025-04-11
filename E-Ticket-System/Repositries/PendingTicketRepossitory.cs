using E_Ticket_System.DataAcess;
using E_Ticket_System.Models;
using E_Ticket_System.Repositries.Irepostries;

namespace E_Ticket_System.Repositries
{
    public class PendingTicketRepossitory : respository<PendingTicket>, IPendingTicketRepository
    {
        private readonly ApplicationDbContext dbContext;
        public PendingTicketRepossitory(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
