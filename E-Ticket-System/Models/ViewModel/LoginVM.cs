using System.ComponentModel.DataAnnotations;

namespace E_Ticket_System.Models.ViewModel
{
    public class LoginVM
    {
       public int Id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public bool rememberme { get; set; }
    }
}
