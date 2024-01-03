using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.Catalog
{
    public class StoreModel
    {
        [Key]
        public int IdStore { get; set; }

        [Required(ErrorMessage = "El campo Nombre de Almacen es obligatorio")]
        [MaxLength(50, ErrorMessage = "Nombre debe ser maximo de 50 caracteres")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "El campo Descripcion de Almacen es obligatorio")]
        [MaxLength(100, ErrorMessage = "Decripcion debe ser maximo de 50 caracteres")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "El Estado es requerido")]
        public bool Estate { get; set; }
    }
}
