using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models.Catalog
{
    public class ProductModel
    {
        [Key]
        public int IdProduct { get; set; }
        [Required(ErrorMessage = "Numero de Serie es requerido")]
        [MaxLength(60)]
        public string? SerialNumber { get; set; }
        [Required(ErrorMessage = "Descripcion del producto es Requerida")]
        [MaxLength(100)]
        public string? Description { get; set; }
        [Required(ErrorMessage = "El precio del producto es requerido")]
        public double Price { get; set; }
        [Required(ErrorMessage = "El Costo del producto es requerido")]
        public double CostPrice { get; set; }

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "El campo estado es requerido")]
        public bool Estate { get; set; }

        //Campos asociados o llaves foreaneas

        [Required(ErrorMessage = "El campo Categori es requerido")]
        public int? IdCategory { get; set; }

        [ForeignKey("IdCategory")]
        public CategoryModel? Category { get; set; }

        [Required(ErrorMessage = "El campo Marca es Requerido")]
        public int? IdBrand { get; set; }

        [ForeignKey("IdBrand")]
        public BrandModel? Brand { get; set; }

        public int? MasterId { get; set; }
        public virtual ProductModel? Master { get; set; }



    }
}
