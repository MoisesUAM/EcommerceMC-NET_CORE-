using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.Catalog
{
    public class OrderDetailsModel
    {
        public int IdOrderDetail { get; set; }
        [Required]
        public int IdOrder { get; set; }
        [Required]
        public int IdProduct { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        //Navegacion
        public OrderModel? Products { get; set; }
        public ProductModel? Orders { get; set; }
    }
}
