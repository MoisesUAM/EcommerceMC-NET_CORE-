using Ecommerce.BLL.Notifications;
using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.Models.Catalog;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System.Security.Claims;

namespace Ecommerce.UI.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize(Roles = DS.AdminRole + "," + DS.EmployeeRole)]
    public class InventoryController : Controller
    {
        private readonly IUnitWork _unitWork;
        public InventoryViewModel? InventoryVM { get; set; }

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
            InventoryVM.DetailsInventoryList = await _unitWork.DetailsInventoryRepository.GetAll(d=>d.IdInventory == id,includedProperties: "Product,Product.Brand");
            if(InventoryVM.DetailsInventoryList != null && InventoryVM.Inventory != null)
            {
                return View(InventoryVM);
            }
            TempData[DS.Error] = "Error al cargar registro de inventario";
            InventoryVM.StoreList = _unitWork.InventoryRepository.SelectListStores();
            return RedirectToAction("CreateInventory", InventoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsInventory(int IdInventory, int idProduct, int cantidadId)
        {
            InventoryVM = new InventoryViewModel();
            InventoryVM.Inventory = await _unitWork.InventoryRepository.GetFirst(i => i.IdInventory == IdInventory);
            var productStore = await _unitWork.StoreProductsRepository.GetFirst(ps=>ps.IdProduct == idProduct && ps.IdStore == InventoryVM.Inventory.IdStore);
            var details = await _unitWork.DetailsInventoryRepository.GetFirst(d=>d.IdInventory == IdInventory && d.IdProducto == idProduct);

            if (details == null) 
            {
                InventoryVM.DetailsInventory = new Models.Catalog.DetailsInventoryModels();
                InventoryVM.DetailsInventory.IdProducto = idProduct;
                InventoryVM.DetailsInventory.IdInventory = IdInventory;

                if(productStore != null)
                {
                    InventoryVM.DetailsInventory.LastStock = productStore.OnHand;
                }
                else
                {
                    InventoryVM.DetailsInventory.LastStock = 0;
                }
                InventoryVM.DetailsInventory.Quantity = cantidadId;
                await _unitWork.DetailsInventoryRepository.Add(InventoryVM.DetailsInventory);
                await _unitWork.Save();
            }
            else
            {
                details.Quantity += cantidadId;
                await _unitWork.Save();
            }

            return RedirectToAction("DetailsInventory", new {id = IdInventory});
        }

        //Metodo para disminuir o aumentar la cantidad del producto en uno

        public async Task<IActionResult> UpDownQuantity(string operation, int id )
        {
            InventoryVM = new InventoryViewModel();
            var detailInventory = await _unitWork.DetailsInventoryRepository.GetById(id);
            InventoryVM.Inventory = await _unitWork.InventoryRepository.GetById(detailInventory.IdInventory);
            if (operation.Equals("sum"))
            {
                detailInventory.Quantity += 1;
                await _unitWork.Save();
            }
            else
            {
                if(detailInventory.Quantity == 1) {
                    _unitWork.DetailsInventoryRepository.Remove(detailInventory);
                    await _unitWork.Save();
                }
                else { 
                detailInventory.Quantity -= 1;
                    await _unitWork.Save();
                }
            }
            return RedirectToAction("DetailsInventory", new { id = InventoryVM.Inventory.IdInventory });
        }

        public async Task<IActionResult> CreateStock(int id)
        {
            var inventory = await _unitWork.InventoryRepository.GetById(id);
            var detailsList = await _unitWork.DetailsInventoryRepository.GetAll(d =>d.IdInventory == id);
            //Se obtiene la identidad del usuario conectado
            var claimIdentity = User.Identity as ClaimsIdentity;
            var claim = claimIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            foreach (var item in detailsList)
            {
                var storeProduct = new StoreProductModel();
                storeProduct = await _unitWork.StoreProductsRepository.GetFirst(p => p.IdProduct == item.IdProducto && p.IdStore == inventory.IdStore);

                if (storeProduct != null)// en esta caso el stock existe y lo que se hace es actualizar las cantidades
                {
                    await _unitWork.TransactionsRepository.RegisterTransaction(storeProduct.IdProduct, storeProduct.IdStore, "IN", "Registro de Inventario", storeProduct.OnHand, item.Quantity, claim!.Value);
                    storeProduct.OnHand += item.Quantity;
                    await _unitWork.Save();
                }
                else //en este caso del stock no existe y se debe crear
                {
                    storeProduct = new StoreProductModel();
                    storeProduct!.IdStore = inventory!.IdStore;
                    storeProduct.IdProduct = item.IdProducto;
                    storeProduct.OnHand += item.Quantity;
                    await _unitWork.StoreProductsRepository.Add(storeProduct);
                    await _unitWork.Save();
                    await _unitWork.TransactionsRepository.RegisterTransaction(storeProduct.IdProduct, storeProduct.IdStore, "IN", "Inventario inicial", item.LastStock, item.Quantity, claim!.Value);
                }
            }

            //Actualizar estado del inventario
            inventory.Estate = true;
            inventory.EndDate = DateTime.Now;
            await _unitWork.Save();
            TempData[DS.Success] = "Stock Generado Exitosamente!";
            return View("Index");
         
        }

        public IActionResult TransactionInventory()
        {
            TransactionsViewModel transactionsViewModel = new TransactionsViewModel();
            transactionsViewModel.TransactionList = null;
            transactionsViewModel.Products = new ProductModel();
            transactionsViewModel.Products.SerialNumber = "";
            transactionsViewModel.Products.Description = "";
            return View(transactionsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> TransactionInventory(string startDate, string endDate, int productList)
        {
            TransactionsViewModel transactionsViewModel = new TransactionsViewModel();
            transactionsViewModel.Products = new ProductModel();
            transactionsViewModel.Products = await _unitWork.ProductRepository.GetById(productList);
            transactionsViewModel.StartDate = DateTime.Parse(startDate);
            transactionsViewModel.EndDate = DateTime.Parse(endDate);
            transactionsViewModel.TransactionList = await _unitWork.TransactionsRepository.GetAll(
                t=>t.StoreProduct!.IdProduct == productList &&
                (t.CommitDate >= transactionsViewModel.StartDate &&
                t.CommitDate <= transactionsViewModel.EndDate),
                includedProperties: "StoreProduct,StoreProduct.Products,StoreProduct.Stores",
                orderBy: o=>o.OrderBy(o=>o.CommitDate)
                );

            return View(transactionsViewModel);
        }

        public async Task<IActionResult> PrintReport(DateTime startDate, DateTime endDate, int productId)
        {
            TransactionsViewModel transactionsViewModel = new TransactionsViewModel();
            transactionsViewModel.Products = new ProductModel();
            transactionsViewModel.Products = await _unitWork.ProductRepository.GetById(productId);
            transactionsViewModel.StartDate = startDate;
            transactionsViewModel.EndDate = endDate;
            transactionsViewModel.TransactionList = await _unitWork.TransactionsRepository.GetAll(
                t => t.StoreProduct!.IdProduct == productId &&
                (t.CommitDate >= transactionsViewModel.StartDate &&
                t.CommitDate <= transactionsViewModel.EndDate),
                includedProperties: "StoreProduct,StoreProduct.Products,StoreProduct.Stores",
                orderBy: o => o.OrderBy(o => o.CommitDate)
                );

            return new ViewAsPdf("PrintReport", transactionsViewModel)
            {
                FileName = "Reporte de Transacciones codigo: " + transactionsViewModel.Products.SerialNumber + ".pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 12",
                ContentType = "text/html"
            };
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> GetAllStoresProducts()
        {
            var allItems = await _unitWork.StoreProductsRepository.GetAll(includedProperties: "Stores,Products");
            return Json(new {data = allItems });
        }

        [HttpGet]
        public async Task<IActionResult> FindProductByTerm(string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                var listProducts = await _unitWork.ProductRepository.GetAll(p => p.Estate == true);
                var data = listProducts.Where(p => p.SerialNumber!.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                                                   p.Description!.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
                return Ok(data);
            }
            return Ok();
        }

        //public async Task<IActionResult> addDetail(int id)
        //{
        //    InventoryVM = new InventoryViewModel();
        //    var detail = await _unitWork.DetailsInventoryRepository.GetById(id);
        //    InventoryVM.Inventory = await _unitWork.InventoryRepository.GetById(detail.IdInventory);
        //    detail.Quantity += 1;
        //    await _unitWork.Save();

        //    return RedirectToAction("DetailsInventory", new {id = InventoryVM.Inventory.IdInventory});
        //}

        //public async Task<IActionResult> DecreaseDetail(int id)
        //{
        //    InventoryVM = new InventoryViewModel();
        //    var detail = await _unitWork.DetailsInventoryRepository.GetById(id);
        //    InventoryVM.Inventory = await _unitWork.InventoryRepository.GetById(detail.IdInventory);
        //    if (detail.Quantity == 1)
        //    {
        //        _unitWork.DetailsInventoryRepository.Remove(detail);
        //        await _unitWork.Save();
        //    }
        //    else
        //    {
        //        detail.Quantity -= 1;
        //    }

        //    return RedirectToAction("DetailsInventory", new { id = InventoryVM.Inventory.IdInventory });
        //}


        #endregion
    }

}
