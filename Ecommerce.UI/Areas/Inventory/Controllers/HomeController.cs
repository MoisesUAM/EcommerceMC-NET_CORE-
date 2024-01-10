using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.Models.Catalog;
using Ecommerce.Models.PaginationSpecs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Ecommerce.UI.Areas.Inventory.Controllers
{
    [Area("Inventory")]
    public class HomeController : Controller
    {
        private readonly IUnitWork _UnitWork;

        public HomeController(IUnitWork unitWork)
        {
            _UnitWork = unitWork;
        }
        public IActionResult Index(int itemsPerPage, int pageNumber, string _search="", string currentSearch ="")
        {
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
