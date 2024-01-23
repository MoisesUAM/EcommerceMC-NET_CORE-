using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.Catalog
{
    public class DetailsInventoryModels
    {
        [Required(ErrorMessage ="El id de detalle es obligatorio")]
        public int IdDetailsIventory { get; set; }

        //llave foreanea con Inventory
        [Required(ErrorMessage = "El id de Inventario es obligatiorio")]
        public int IdInventory { get; set; }
        //llave foranea con Prouctos
        [Required(ErrorMessage = "El id de produto es obligatorio")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El campo ultima cantidad es obligatorio")]
        public int LastStock { get; set; }
        [Required(ErrorMessage = "El campo cantidad es obligatorio")]
        public int Quantity { get; set; }

        //Navegaciones
        public InventoryModel? Inventory { get; set; }
        public ProductModel? Product { get; set; }
    }
}
