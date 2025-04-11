using E_Ticket_System.DataAcess;
using E_Ticket_System.Models;
using E_Ticket_System.Repositries.Irepostries;

namespace E_Ticket_System.Repositries
{
    public class ActorMoviesRepository:respository<ActorMovies>,IActorMoviesRepository
    {
        private readonly ApplicationDbContext dbContext;
        public ActorMoviesRepository(ApplicationDbContext dbContext) : base(dbContext) {
            this.dbContext = dbContext;
        }
        
            
        }
    }

