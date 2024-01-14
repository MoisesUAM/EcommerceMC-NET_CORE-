using Ecommerce.BLL.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;

namespace Ecommerce.UI.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class TestController : Controller
    {

        public ActionResult Index()
        {
            TempData["message"] = "Bienvenido";

            return View();
        }

        [HttpPost]
        public IActionResult DataTest([FromBody] string? url)
        {
            string returnUrl = Url.Content("~/");
            string host = url;

            if(url == null)
            {
                host = "vacio";
                return Json(new { success = true, message = host });
            }
            else
            {
                var callbackUrl = Url.Page(
                     "/Identity/Account",
                     pageHandler: null,
                     values: new { area = "Identity", userId = "mcoto", code = "", returnUrl = returnUrl },
                     protocol: HttpContext.Request.Scheme, host: host);
                return Json(new { success = true, message = host });
            }



        }
    }
}

public class TestModel()
{
    public string? RequestData { get; set; }
}
