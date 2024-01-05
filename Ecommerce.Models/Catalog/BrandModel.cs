using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Models.Catalog
{
    public class BrandModel
    {

        [Key]
        public int IdBrand { get; set; }

        [Required(ErrorMessage ="El nombre de la marca es requerido")]
        [MaxLength(60)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "La descripcion de la marca es requerida")]
        [MaxLength(100)]
        public string? Description { get; set; }

        [Required(ErrorMessage ="El estado es requerido")]
        public bool Estate { get; set; }
    }
}
