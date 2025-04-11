using E_Ticket_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using E_Ticket_System.Models.ViewModel;

namespace E_Ticket_System.DataAcess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

    {
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Actor> Actor { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Cinema> Cinema { get; set; }
        public DbSet<ActorMovies> ActorMovies { get; set; }
        public DbSet<PendingTicket> pendingTickets { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }
        public ApplicationDbContext()
        {

        }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=E-Tickets;Integrated Security=True;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>().ToTable("Movie");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Cinema>().ToTable("Cinema");
            modelBuilder.Entity<Actor>().ToTable("Actor");
            modelBuilder.Entity<ActorMovies>().ToTable("ActorMovies");
            modelBuilder.Entity<ActorMovies>()
                   .HasKey(am => new { am.ActorId, am.MovieId });
           
           

            modelBuilder.Entity<PendingTicket>()
      .HasOne(pt => pt.Movie)
      .WithMany()
      .HasForeignKey(pt => pt.MovieId)
      .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<PendingTicket>()
                .HasOne(pt => pt.Cinema)
                .WithMany()
                .HasForeignKey(pt => pt.CinemaId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<PendingTicket>()
                .HasOne(pt => pt.User)
                .WithMany()
                .HasForeignKey(pt => pt.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

        }
        public DbSet<E_Ticket_System.Models.ViewModel.RegisterVm> RegisterVm { get; set; } = default!;
        public DbSet<E_Ticket_System.Models.ViewModel.LoginVM> LoginVM { get; set; } = default!;
    }
}
