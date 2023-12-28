using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
