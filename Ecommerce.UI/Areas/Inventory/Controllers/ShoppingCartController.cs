using Ecommerce.BLL.Notifications;
using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.Models.Catalog;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace Ecommerce.UI.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    [Authorize(Roles =DS.ClientRole+","+DS.EmployeeRole+","+DS.AdminRole)]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitWork _unitWork;
        private string? _webUrlReturn;

        [BindProperty]
        public ShoppinCartViewModel shoppinCartVM { get; set; }

        public ShoppingCartController(IUnitWork unitWork, IConfiguration config)
        {
            _unitWork = unitWork;
            _webUrlReturn = config.GetValue<string>("DomainUrls:WEB_URL");
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

        public async Task<IActionResult> Checkout()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppinCartVM = new ShoppinCartViewModel()
            {
                Order = new Models.Catalog.OrderModel(),
                ShopingCartList = await _unitWork.ShoppinCartRepository.GetAll(u => u.UserId == claim.Value, includedProperties: "Products"),
                Company = await _unitWork.CompanyRepository.GetFirst()
            };

            shoppinCartVM.Order.TotalOrderAmount = 0;
            shoppinCartVM.Order.Users = await _unitWork.UserModelRepository.GetFirst(u=>u.Id == claim.Value);

            foreach(var list in shoppinCartVM.ShopingCartList)
            {
                list.Price = list.Products.Price;
                shoppinCartVM.Order.TotalOrderAmount += (list.Price * list.Quantity);
            }

            shoppinCartVM.Order.ClientName = shoppinCartVM.Order.Users.Name + " " + shoppinCartVM.Order.Users.LastName;
            shoppinCartVM.Order.ShippingAddress = shoppinCartVM.Order.Users.Address;
            shoppinCartVM.Order.Country = shoppinCartVM.Order.Users.Country;
            shoppinCartVM.Order.City = shoppinCartVM.Order.Users.City;
            shoppinCartVM.Order.PhoneNumber = shoppinCartVM.Order.Users.PhoneNumber;

            //controlar el stock
            foreach (var list in shoppinCartVM.ShopingCartList)
            {
                //capturar el stock de cada producto
                var product = await _unitWork.StoreProductsRepository.GetFirst(p => p.IdProduct == list.IdProduct && p.IdStore == shoppinCartVM.Company.IdStore);
                
                if(list.Quantity > product.OnHand)
                {
                    TempData[DS.Error] = "Atencion!! la cantidad del producto " + list.Products.Description + " " + " Excede al disponible actual (" + product.OnHand + ")";
                    return RedirectToAction("Index");
                }
            }

            return View(shoppinCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(ShoppinCartViewModel shoppinCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppinCartVM.ShopingCartList = await _unitWork.ShoppinCartRepository.GetAll(c=>c.UserId == claim.Value, includedProperties:"Products");

            shoppinCartVM.Company = await _unitWork.CompanyRepository.GetFirst();
            shoppinCartVM.Order.TotalOrderAmount = 0;
            shoppinCartVM.Order.IdUser = claim.Value;
            shoppinCartVM.Order.OrderDate = DateTime.Now;

            foreach(var list in shoppinCartVM.ShopingCartList)
            {
                list.Price = list.Products.Price;
                shoppinCartVM.Order.TotalOrderAmount += (list.Price * list.Quantity);
            }

            //controlar el stock
            foreach (var list in shoppinCartVM.ShopingCartList)
            {
                //capturar el stock de cada producto
                var product = await _unitWork.StoreProductsRepository.GetFirst(p => p.IdProduct == list.IdProduct && p.IdStore == shoppinCartVM.Company.IdStore);

                if (list.Quantity > product.OnHand)
                {
                    TempData[DS.Error] = "Atencion!! la cantidad del producto " + list.Products.Description + " " + " Excede al disponible actual (" + product.OnHand + ")";
                    return RedirectToAction("Index");
                }
            }

            shoppinCartVM.Order.OrderState = DS.PendingOrder;
            shoppinCartVM.Order.PaymentState = DS.PendingPayment;
            await _unitWork.OrderRepository.Add(shoppinCartVM.Order);
            await _unitWork.Save();

            //guardar detalle de la orden

            foreach (var list in shoppinCartVM.ShopingCartList)
            {
                OrderDetailsModel orderDetails = new OrderDetailsModel()
                {
                    IdProduct = list.IdProduct,
                    IdOrder = shoppinCartVM.Order.IdOrder,
                    Price = list.Price,
                    Quantity = list.Quantity
                };

                await _unitWork.OrderDetailRepository.Add(orderDetails);
                await _unitWork.Save();

            }

            //Stripe process
            var userEmail = await _unitWork.UserModelRepository.GetFirst(u => u.Id == claim.Value);
            var options = new SessionCreateOptions
            {
                SuccessUrl = _webUrlReturn + $"Inventory/ShoppingCart/OrderConfirmation?id={shoppinCartVM.Order.IdOrder}",
                CancelUrl = _webUrlReturn + "Inventory/ShoppingCart/Index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = userEmail.Email
            };

            foreach (var list in shoppinCartVM.ShopingCartList)
            {
                var sessionLineitem = new SessionLineItemOptions { 
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)(list.Price*100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = list.Products.Description
                        }
                    },
                    Quantity = list.Quantity
                };

                options.LineItems.Add(sessionLineitem);
            }
            var services = new SessionService();
            Session session = services.Create(options);
            _unitWork.OrderRepository.UpdatePaymentStateStripe(shoppinCartVM.Order.IdOrder, session.Id, session.PaymentIntentId);
            await _unitWork.Save();
            Response.Headers.Add("Location", session.Url); //redirecciona al sitio de Stripe
            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _unitWork.OrderRepository.GetFirst(o=>o.IdOrder == id, includedProperties:"Users");
            var services = new SessionService();
            Session session = services.Get(order.SessionId);
            var shoppinCart = await _unitWork.ShoppinCartRepository.GetAll(u => u.UserId == order.IdUser);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitWork.OrderRepository.UpdatePaymentStateStripe(id, session.Id, session.PaymentIntentId);
                _unitWork.OrderRepository.UpdateStateOrder(id, DS.ApprovedOrder, DS.ApprovedPayment);
                await _unitWork.Save();

                //Disminuir el stock
                var company = await _unitWork.CompanyRepository.GetFirst();
                foreach (var list in shoppinCart)
                {
                    var storeProduct = new StoreProductModel();
                    storeProduct = await _unitWork.StoreProductsRepository.GetFirst(b=>b.IdProduct == list.IdProduct && b.IdStore == company.IdStore);
                    await _unitWork.TransactionsRepository.RegisterTransaction(storeProduct.IdProduct, storeProduct.IdStore,
                                                                               "OUT", "Pedido de venta orden# "+id,
                                                                               storeProduct.OnHand,
                                                                               list.Quantity,
                                                                               order.IdUser);
                    storeProduct.OnHand -= list.Quantity;
                    await _unitWork.Save();
                }

            }

            //Borrar el carrito de compras
           
            List<ShoppingCartModel> shoppingCartList  = shoppinCart.ToList();
            _unitWork.ShoppinCartRepository.DeleteRange(shoppingCartList);
            await _unitWork.Save();
            HttpContext.Session.SetInt32(DS.SesionShoppingCart, 0);

            return View(id);
        }
    }
}
