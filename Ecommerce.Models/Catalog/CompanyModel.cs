using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.Catalog
{
    public class CompanyModel
    {
        public int IdCompany { get; set; }
        [Required(ErrorMessage ="Campo Nombre requerido")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Campo Descripcion requerido")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Campo Pais requerido")]
        public string? Country { get; set; }
        [Required(ErrorMessage = "Campo Cuidad requerido")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Campo Direccion requerido")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Campo Telefono requerido")]
        public string? PhoneNumber { get; set; }
        public int IdStore { get; set; }
        public StoreModel? Store { get; set; }
        public string? CreateUserId { get; set; }
        public UserModel? CreateUser { get; set; }
        public string? UpdateUserId { get; set; }
        public UserModel? UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
