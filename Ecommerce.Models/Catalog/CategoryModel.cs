using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Catalog
{
    public class CategoryModel
    {
        [Key]
        public int IdCategory { get; set; }

        [Required(ErrorMessage = "El campo Nombre de Categoria es requerido")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo Descripcion de Categoria es requerido")]
        [MaxLength(100)]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo Estado de Categoria es requerido")]
        public bool Estate { get; set; }
    }
}
