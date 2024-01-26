using Ecommerce.BLL.Notifications;
using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.Models.Catalog;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =DS.AdminRole+","+DS.EmployeeRole+","+DS.ClientRole)]
    public class OrderController : Controller
    {

        private readonly IUnitWork unitWork;

        [BindProperty]
        public OrderDetailsViewModel OrderDetailsVM { get; set; }

        public OrderController(IUnitWork _unitWork)
        {
            unitWork = _unitWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders(string status)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<OrderModel> orders;

            if(User.IsInRole(DS.EmployeeRole) || User.IsInRole(DS.AdminRole))
            {
                orders = await unitWork.OrderRepository.GetAll(includedProperties: "Users");
            }
            else
            {
                orders = await unitWork.OrderRepository.GetAll(u=>u.IdUser == claim.Value, includedProperties: "Users");
            }

            //asignar estado
            switch (status)
            {
                case "approved":
                    orders = orders.Where(o => o.OrderState == DS.ApprovedOrder);
                    break;
                case "completed":
                    orders = orders.Where(o => o.OrderState == DS.OrderShipped);
                    break;
                default:
                    break;
            }

            return Json(new {data = orders});
        }

        public async Task<IActionResult> Details(int id)
        {
            OrderDetailsVM = new OrderDetailsViewModel()
            {
                Order = await unitWork.OrderRepository.GetFirst(o=>o.IdOrder == id, includedProperties: "Users"),
                OrderDetailList = await unitWork.OrderDetailRepository.GetAll(d=>d.IdOrder == id, includedProperties:"Products")
            };

            return View(OrderDetailsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details()
        {
            return View();
        }

        [Authorize(Roles =DS.AdminRole)]
        public async Task<IActionResult> ProcessOrder(int id) 
        {
            var order = await unitWork.OrderRepository.GetFirst(o => o.IdOrder == id);
            order.OrderState = DS.OrderProcessing;
            await unitWork.Save();
            TempData[DS.Success] = "Procesando Orden " + id;
            return RedirectToAction("Details", new {id = id});
        }

        
        [HttpPost]
        [Authorize(Roles = DS.AdminRole)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendOrder(OrderDetailsViewModel orderVM)
        {
            var order = await unitWork.OrderRepository.GetFirst(o => o.IdOrder == orderVM.Order!.IdOrder);
            order.OrderState = DS.OrderShipped;
            order.Carrier = orderVM.Order!.Carrier;
            order.TrackingNumber = orderVM.Order!.TrackingNumber;
            order.ShippingDate = DateTime.Now;
            await unitWork.Save();
            TempData[DS.Success] = "Se envio la orden " + orderVM.Order.IdOrder;
            return RedirectToAction("Details", new { id = orderVM.Order.IdOrder });
        }
    }
}
