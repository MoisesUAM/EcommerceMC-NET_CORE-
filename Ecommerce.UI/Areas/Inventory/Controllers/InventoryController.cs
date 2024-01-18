using Ecommerce.BLL.Notifications;
using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.UI.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize(Roles = DS.AdminRole+","+DS.EmployeeRole)]
    public class InventoryController : Controller
    {
        private readonly IUnitWork _unitWork;
        public InventoryViewModel InventoryVM { get; set; }

        public InventoryController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateInventory() 
        {
            InventoryVM = new InventoryViewModel()
            {
                Inventory = new Models.Catalog.InventoryModel(),
                StoreList = _unitWork.InventoryRepository.SelectListStores()
            };
            InventoryVM.Inventory.Estate = false;

            //Se obtiene la identidad del usuario conectado
            var claimIdentity = User.Identity as ClaimsIdentity;
            var claim = claimIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            InventoryVM.Inventory.UserId = claim!.Value;
            InventoryVM.Inventory.StartDate = DateTime.UtcNow;
            InventoryVM.Inventory.EndDate = DateTime.UtcNow;
            return View(InventoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInventory(InventoryViewModel _inventoryVM)
        {
            if(ModelState.IsValid && _inventoryVM.Inventory!.IdStore != 0)
            {
                _inventoryVM.Inventory!.StartDate = DateTime.Now;
                _inventoryVM.Inventory.EndDate = DateTime.Now;
                await _unitWork.InventoryRepository.Add(_inventoryVM.Inventory);
                await _unitWork.Save();
                return RedirectToAction("DetailsInventory", new { id = _inventoryVM.Inventory.IdInventory });
            }

            if(_inventoryVM.Inventory!.IdStore == 0)
            {
                TempData[DS.Error] = "Debe Seleccionar un Almacen";
            }
            else
            {
                TempData[DS.Error] = "Error al procesar los datos";
            }
            _inventoryVM.StoreList = _unitWork.InventoryRepository.SelectListStores();
            return View(_inventoryVM);
        }

        public async Task<IActionResult> DetailsInventory(int id)
        {
            InventoryVM = new InventoryViewModel();
            InventoryVM.Inventory = await _unitWork.InventoryRepository.GetFirst(i=>i.IdInventory == id, includedProperties:"Stores");
            InventoryVM.DetailsInventoryList = await _unitWork.DetailsInventoryRepository.GetAll(d=>d.IdInventory == id,includedProperties: "Product.Brand");
            return View(InventoryVM);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> GetAllInventories()
        {
            var allItems = await _unitWork.InventoryRepository.GetAll(includedProperties:"Users,Stores");
            return Json(new {data = allItems });
        }

        [HttpGet]
        public async Task<IActionResult> FindProductByTerm(string term)
        {
            if (String.IsNullOrEmpty(term))
            {
                var listProducts = await _unitWork.ProductRepository.GetAll(p => p.Estate == true);
                var data = listProducts.Where(p => p.SerialNumber!.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                                                   p.Description!.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
                return Ok(data);
            }
            return Ok();
        }



        #endregion
    }
}
