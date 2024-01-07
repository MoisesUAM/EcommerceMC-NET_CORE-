using Ecommerce.Models.Catalog;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Models.ViewModels
{
    public class ProductViewModel
    {
        public ProductModel? Product { get; set; }
        public IEnumerable<SelectListItem>? BrandSelectItemList { get; set; }
        public IEnumerable<SelectListItem>? CategorySelectItemList { get; set; }
        public IEnumerable<SelectListItem>? ProductSelectItemList { get; set; }
    }
}

