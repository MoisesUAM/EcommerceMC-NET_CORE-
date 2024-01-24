using Ecommerce.BLL.Notifications;
using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.Models.Catalog;
using Ecommerce.Models.PaginationSpecs;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Ecommerce.UI.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class HomeController : Controller
    {
        private readonly IUnitWork _UnitWork;

        [BindProperty]
        public ShoppinCartViewModel ShoppinCartVM { get; set; }

        public HomeController(IUnitWork unitWork)
        {
            _UnitWork = unitWork;
        }
        public async Task<IActionResult> Index(int itemsPerPage, int pageNumber, string _search="", string currentSearch ="")
        {
            //Controlar sesion
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claims!= null)
            {
                var shoppingCartList = await _UnitWork.ShoppinCartRepository.GetAll(s => s.UserId == claims.Value);
                HttpContext.Session.SetInt32(DS.SesionShoppingCart, shoppingCartList.Count());
            }

            if (!String.IsNullOrEmpty(_search))
            {
                pageNumber = 1;
            }
            else
            {
                _search = currentSearch;
            }
            ViewData["CurrentSearch"] = _search;

           // var totalRecords =  _UnitWork.ProductRepository.GetAll();
            if (pageNumber < 1) { pageNumber = 1; }
            if (itemsPerPage < 1) {  itemsPerPage = 4; }

            PageParameters parameters = new PageParameters()
            {
                PageNumber = pageNumber,
                PageSize = itemsPerPage
                //TotalRecords = totalRecords.Result.Count()
            };

            var _results = _UnitWork.ProductRepository.GetAllPaginatedItems(parameters);
            if(!String.IsNullOrEmpty(_search))
            {
                _results = _UnitWork.ProductRepository.GetAllPaginatedItems(parameters, s=>s.Description!.Contains(_search));
            }
            ViewData["TotalPages"] = _results.PageMetaData!.TotalPages;
            ViewData["TotalRecords"] = _results.PageMetaData!.TotalCount;
            ViewData["PageSize"] = _results.PageMetaData!.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previous"] = "disabled";
            ViewData["FirtsPage"] = "";
            ViewData["LastPage"] = "";
            ViewData["ItemsPerPage"] = itemsPerPage;
            ViewData["Next"] = "";

            if(pageNumber > 1) { ViewData["Previous"] = ""; }
            if(_results.PageMetaData.TotalPages <= pageNumber) { ViewData["Next"] = "disabled"; }
            if(_results.PageMetaData!.TotalPages == 1) { ViewData["FirtsPage"] = "disabled"; ViewData["LastPage"] = "disabled"; }

            return View(_results);
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            ShoppinCartVM = new ShoppinCartViewModel();
            ShoppinCartVM.Company = await _UnitWork.CompanyRepository.GetFirst();
            ShoppinCartVM.Product = await _UnitWork.ProductRepository.GetFirst(p=>p.IdProduct == id, includedProperties: "Brand,Category");
            var storeProduct = await _UnitWork.StoreProductsRepository.GetFirst(p=>p.IdProduct == id && p.IdStore == ShoppinCartVM.Company.IdStore);
            if(storeProduct == null)
            {
                ShoppinCartVM.Stock = 0;
            }
            else
            {
                ShoppinCartVM.Stock = storeProduct!.OnHand;

            }
            ShoppinCartVM.ShoppingCart = new ShoppingCartModel()
            {
                Products = ShoppinCartVM.Product,
                IdProduct = ShoppinCartVM.Product.IdProduct
            };

            return View(ShoppinCartVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ProductDetails(ShoppinCartViewModel shoppinCartVm)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppinCartVm.ShoppingCart.UserId = claims!.Value;
            string userApp = claims!.Value;



            ShoppingCartModel cartDB = await _UnitWork.ShoppinCartRepository.GetFirst(c => c.UserId == userApp && c.IdProduct == shoppinCartVm.ShoppingCart.IdProduct);
            if(cartDB == null)
            {
                await _UnitWork.ShoppinCartRepository.Add(shoppinCartVm.ShoppingCart);
            }
            else
            {
                cartDB.Quantity += shoppinCartVm.ShoppingCart.Quantity;
                _UnitWork.ShoppinCartRepository.Update(cartDB);
            }

            await _UnitWork.Save();

            TempData[DS.Success] = "Se agrego el producto al carrito de compras";
            //agregar valor a la sesion
            var shoppingCartList = await _UnitWork.ShoppinCartRepository.GetAll(s => s.UserId == userApp);
            HttpContext.Session.SetInt32(DS.SesionShoppingCart, shoppingCartList.Count());
            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
