using E_Ticket_System.DataAcess;
using E_Ticket_System.Models;
using E_Ticket_System.Repositries.Irepostries;

namespace E_Ticket_System.Repositries
{
    public class ActorRepository:respository<Actor>,IActorRepository
    {
        private readonly ApplicationDbContext dbContext;
        public ActorRepository(ApplicationDbContext dbContext) : base(dbContext) { 
        this.dbContext = dbContext;
        }
        
            
        }
    }

