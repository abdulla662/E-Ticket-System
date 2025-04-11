using E_Ticket_System.DataAcess;
using E_Ticket_System.Models;
using E_Ticket_System.Repositries.Irepostries;

namespace E_Ticket_System.Repositries
{
    public class CinemaRepository:respository<Cinema>,IcinemaReposirotry
    {
        private readonly ApplicationDbContext dbContext;
        public CinemaRepository(ApplicationDbContext dbContext) : base(dbContext) { 
        this.dbContext = dbContext;
        }
        
            
        }
    }

