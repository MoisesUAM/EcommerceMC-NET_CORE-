using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models.Catalog
{
    public class ShoppingCartModel
    {
        public int IdShoppingCart { get; set; }
        [Required(ErrorMessage ="Campo Usuario es requerido")]
        public string? UserId { get; set; }
        [Required]
        public int IdProduct { get; set; }
        [Required]
        public int Quantity { get; set; }
        [NotMapped]
        public double Price { get; set; }

        //navegaciones

        public UserModel? Users { get; set; }
        public ProductModel? Products { get; set; }
    }
}
