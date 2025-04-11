using E_Ticket_System.DataAcess;
using E_Ticket_System.Models;
using E_Ticket_System.Repositries.Irepostries;

namespace E_Ticket_System.Repositries
{
    public class CategoryRepository:respository<Category>,ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext) { 
        this.dbContext = dbContext;
        }
        
            
        }
    }

