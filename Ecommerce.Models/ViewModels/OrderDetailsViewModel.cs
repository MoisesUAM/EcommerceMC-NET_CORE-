using Ecommerce.Models.Catalog;

namespace Ecommerce.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        public CompanyModel? Company { get; set; }
        public OrderModel? Order { get; set; }
        public IEnumerable<OrderDetailsModel>? OrderDetailList { get; set; }
    }
}
