namespace Ecommerce.Models.Catalog
{
    public class DetailsInventoryModels
    {
        public int IdDetailsIventory { get; set; }

        //llave foreanea con Inventory
        public int IdInventory { get; set; }
        //llave foranea con Prouctos
        public int IdProducto { get; set; }
        public int LastStock { get; set; }
        public int Quantity { get; set; }

        //Navegaciones
        public InventoryModel? Inventory { get; set; }
        public ProductModel? Product { get; set; }
    }
}
