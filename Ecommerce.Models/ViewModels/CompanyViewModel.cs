using Ecommerce.Models.Catalog;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Models.ViewModels
{
    public class CompanyViewModel
    {
        public CompanyModel? Company { get; set; }
        public IEnumerable<SelectListItem>? StoreListItem { get; set; }
    }
}
