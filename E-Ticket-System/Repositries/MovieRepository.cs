using E_Ticket_System.DataAcess;
using E_Ticket_System.Models;
using E_Ticket_System.Repositries.Irepostries;

namespace E_Ticket_System.Repositries
{
    public class MovieRepository:respository<Movie>,ImovieRepository
    {
        private readonly ApplicationDbContext dbContext;


        public MovieRepository(ApplicationDbContext dbContext) : base(dbContext) { 
        this.dbContext = dbContext;
        }
        
            
        }
    }

