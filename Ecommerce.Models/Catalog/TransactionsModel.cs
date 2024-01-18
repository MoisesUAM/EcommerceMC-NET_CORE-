namespace Ecommerce.Models.Catalog
{
    public class TransactionsModel
    {

        public int IdTransaction { get; set; }
        
        //llave foranea con StoreProduct se combinan los id de Store y product en 
        //la configuracion aca se ponen separados
        public int IdStore { get; set; }
        public int IdProduct { get; set; }
        //Lave foranea con Users
        public string? UserId { get; set; }
        public string? Type { get; set; }
        public string? Comments { get; set; }
        public int LastStock { get; set; }
        public int CurrentStock { get; set; }
        public int Quantity { get; set;}
        public double Cost { get; set;}
        public double CostAmount { get;}
        public DateTime CommitDate { get; set; }

        //Navegaciones

        public StoreProductModel? StoreProduct { get; set; }
        public UserModel? User { get; set; }


    }
}
