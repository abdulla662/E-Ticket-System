using E_Ticket_System.DataAcess;
using E_Ticket_System.Models;

namespace E_Ticket_System.Repositries.Irepostries
{
    public class ApplicationUserRepository : respository<ApplicationUser>, IApplicationUserReposatory
    {
        public ApplicationUserRepository(ApplicationDbContext applicationDb) : base(applicationDb)
        {
        }
    }
}