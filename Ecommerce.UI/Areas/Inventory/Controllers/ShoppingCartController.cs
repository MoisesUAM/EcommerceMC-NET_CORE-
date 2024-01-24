using Ecommerce.BLL.Notifications;
using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.UI.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize(Roles =DS.ClientRole+","+DS.EmployeeRole+","+DS.AdminRole)]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitWork _unitWork;
        [BindProperty]
        public ShoppinCartViewModel shoppinCartVM { get; set; }

        public ShoppingCartController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }

        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppinCartVM = new ShoppinCartViewModel();
            shoppinCartVM.Order = new Models.Catalog.OrderModel();
            shoppinCartVM.ShopingCartList = await _unitWork.ShoppinCartRepository.GetAll(u=>u.UserId == claim.Value, includedProperties:"Products");
            shoppinCartVM.Order.TotalOrderAmount = 0;
            shoppinCartVM.Order.IdUser = claim.Value;

            foreach(var list in shoppinCartVM.ShopingCartList)
            {
                list.Price = list.Products.Price; //siempre tendra el precio actualizado
                shoppinCartVM.Order.TotalOrderAmount += (list.Price * list.Quantity);
            }

            return View(shoppinCartVM);
        }

        public async Task<IActionResult> IncreaseUnitItem(int shoppinCartId)
        {
            var shoppingCard = await _unitWork.ShoppinCartRepository.GetFirst(s =>s.IdShoppingCart == shoppinCartId);
            var shoppingCartList = await _unitWork.ShoppinCartRepository.GetAll(u => u.UserId == shoppingCard.UserId);
            var productsCount = shoppingCartList.Count();

            if (shoppingCard.Quantity == 1)
            {

                productsCount = productsCount - 1;
                HttpContext.Session.SetInt32(DS.SesionShoppingCart, productsCount);
                _unitWork.ShoppinCartRepository.Remove(shoppingCard);
            }

            if(productsCount == 0)
            {
                await _unitWork.Save();
                return RedirectToAction("Index", "Home");
            }

            shoppingCard.Quantity -= 1;
            await _unitWork.Save();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveItem(int shoppinCartId)
        {
            var shoppingCard = await _unitWork.ShoppinCartRepository.GetFirst(s => s.IdShoppingCart == shoppinCartId);
            var shoppingCartList = await _unitWork.ShoppinCartRepository.GetAll(u => u.UserId == shoppingCard.UserId);
            var productsCount = shoppingCartList.Count();

            productsCount = productsCount - 1;
            HttpContext.Session.SetInt32(DS.SesionShoppingCart, productsCount - 1);
            _unitWork.ShoppinCartRepository.Remove(shoppingCard);
  

            if (productsCount == 0)
            {
                await _unitWork.Save();
                return RedirectToAction("Index","Home");
            }
            await _unitWork.Save();
            return View(shoppingCard);
        }

        public async Task<IActionResult> DecreaseUnitItem(int shoppinCartId)
        {
            var shoppingCard = await _unitWork.ShoppinCartRepository.GetFirst(s => s.IdShoppingCart == shoppinCartId);
            shoppingCard.Quantity += 1;
            await _unitWork.Save();
            return RedirectToAction("Index");
        }
    }
}
