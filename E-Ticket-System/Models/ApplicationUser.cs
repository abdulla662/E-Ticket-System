using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace E_Ticket_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Adress { get; set; }
        public string? ProfilePicture { get; set; }


    }
}
