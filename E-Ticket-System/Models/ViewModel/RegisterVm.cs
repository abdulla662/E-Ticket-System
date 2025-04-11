using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace E_Ticket_System.Models.ViewModel
{
    public class RegisterVm
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]   
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public string? Adress {  get; set; }
    }
}
