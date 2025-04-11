using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Ticket_System.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required,MinLength(3)]
        public string Name { get; set; }

        [ValidateNever]
        public List<Movie> Movies { get; set; }

    }
}
