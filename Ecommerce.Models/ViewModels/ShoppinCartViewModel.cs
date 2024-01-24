using Ecommerce.Models.Catalog;

namespace Ecommerce.Models.ViewModels
{
    public class ShoppinCartViewModel
    {
        public CompanyModel? Company { get; set; }
        public ProductModel? Product { get; set; }
        public int Stock { get; set; }
        public ShoppingCartModel? ShoppingCart { get; set; }
        public IEnumerable<ShoppingCartModel>? ShopingCartList { get; set; }
        public OrderModel? Order { get; set; }
    }
}
