using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.Catalog
{
    public class InventoryModel
    {
        public int IdInventory { get; set; }

        //Llave foranea con Usuarios
        public string? UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //llave foranea con Store
        public int IdStore { get; set; }
        public bool Estate { get; set; }

        //Navegaciones con Usuario y Store

        public UserModel? Users { get; set; }
        public StoreModel? Stores { get; set; }
    }
}
