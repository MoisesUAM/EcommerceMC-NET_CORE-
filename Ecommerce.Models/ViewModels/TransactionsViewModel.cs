using Ecommerce.Models.Catalog;

namespace Ecommerce.Models.ViewModels
{
    public class TransactionsViewModel
    {
        public ProductModel? Products { get; set; }
        public IEnumerable<TransactionsModel>? TransactionList { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
