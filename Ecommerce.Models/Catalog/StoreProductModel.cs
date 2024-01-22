using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.Catalog
{
    public class StoreProductModel
    {
        /*
       * Laves de Bodega y producto que formaran la relacion
       * muchos a muchos por medio de esta tabla
       */
        public int IdStore { get; set; }
        public int IdProduct { get; set; }

        public int OnHand { get; set; }

        /*
         * Navegaciones que se usaran para llamar los datos de las tablas
         * relacionadas
         */
        public ProductModel Products { get; set; }
        public StoreModel Stores { get; set; }
    }
}
