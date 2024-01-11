
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models.Catalog
{
    public class UserModel :IdentityUser
    {
        [Required(ErrorMessage ="El Nombre completo es requerido")]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Los Apellidos son requeridos")]
        [MaxLength(100)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "La direccion es requerida")]
        [MaxLength(200)]
        public string? Address { get; set; }

        [Required(ErrorMessage = "La cuidad o provincia es requerida")]
        [MaxLength(100)]
        public string? City { get; set; }

        [Required(ErrorMessage = "El pais de residencia es requerido")]
        [MaxLength(100)]
        public string? Country { get; set; }

        [NotMapped]
        public string Role {  get; set; }    
    }
}
