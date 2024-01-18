using Ecommerce.Models.Catalog;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Models.ViewModels
{
    public  class InventoryViewModel
    {
        public InventoryModel? Inventory { get; set; }
        public DetailsInventoryModels? DetailsInventory { get; set; }
        //Listas para llenado de los selectList
        public IEnumerable<DetailsInventoryModels>? DetailsInventoryList { get; set; }
        public IEnumerable<SelectListItem>? StoreList { get; set; }
    }
}
