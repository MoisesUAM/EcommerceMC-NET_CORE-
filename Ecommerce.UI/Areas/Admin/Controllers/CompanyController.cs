using Ecommerce.BLL.Notifications;
using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =DS.AdminRole)]
    public class CompanyController : Controller
    {
        private readonly IUnitWork _UnitWork;

        public CompanyController(IUnitWork unitWork)
        {
            _UnitWork = unitWork;
        }
       
        public async Task<IActionResult> Upsert()
        {
            CompanyViewModel companyVM = new CompanyViewModel()
            {
                Company = new Models.Catalog.CompanyModel(),
                StoreListItem = _UnitWork.InventoryRepository.SelectListStores()
            };

            companyVM.Company = await _UnitWork.CompanyRepository.GetFirst();

            if (companyVM.Company == null) 
            { 
                companyVM.Company = new Models.Catalog.CompanyModel();
            }

            return View(companyVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CompanyViewModel companyVM)
        {
            if(ModelState.IsValid)
            {
                var claimIdentity = (ClaimsIdentity?)User.Identity;
                var claim = claimIdentity!.FindFirst(ClaimTypes.NameIdentifier);
                if (companyVM.Company!.IdCompany == 0)
                {
                    companyVM.Company.CreateUserId = claim!.Value;
                    companyVM.Company.UpdateUserId = claim.Value;
                    companyVM.Company.CreationDate = DateTime.Now;
                    companyVM.Company.UpdateDate = DateTime.Now;
                    await _UnitWork.CompanyRepository.Add(companyVM.Company);
                    TempData[DS.Success] = "Compañia guardada con exito";
                }
                else
                {
                    companyVM.Company.UpdateUserId = claim!.Value;
                    companyVM.Company.UpdateDate= DateTime.Now;
                    _UnitWork.CompanyRepository.Update(companyVM.Company);
                    TempData[DS.Success] = "Compañia actualiza con exito";
                }

                await _UnitWork.Save();
                return RedirectToAction("Index","Home", new {area="Inventory"});

                
            }
            companyVM.StoreListItem = _UnitWork.InventoryRepository.SelectListStores();
            TempData[DS.Error] = "Error al procesar la compañia";
            return View(companyVM);
        }
    }
}
