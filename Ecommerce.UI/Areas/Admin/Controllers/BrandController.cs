using Ecommerce.BLL.Notifications;
using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitWork _UnitWork;

        public BrandController(IUnitWork unitWork)
        {
            _UnitWork = unitWork ?? throw new ArgumentNullException(nameof(unitWork));
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Upsert(int? id)
        {
            BrandModel brandModel = new BrandModel();

            if (id == null)
            {
                //Aca se trata de una insercion de nuevo almacen
                brandModel.Estate = true;
                return View(brandModel);
            }

            //Aca se trata de una actualizacion de almacen existente
            brandModel = await _UnitWork.BrandRepository.GetById(id.GetValueOrDefault());
            if (brandModel == null)
            {
                return NotFound();
            }

            return View(brandModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upsert(BrandModel brandModel)
        {
            if (!await CheckName(brandModel))
            {
                if (ModelState.IsValid)
                {
                    if (brandModel.IdBrand == 0)
                    {
                        //Se crea un nuevo almacen
                        await _UnitWork.BrandRepository.Add(brandModel);
                        TempData[DS.Success] = "Marca creada correctamente";
                    }
                    else
                    {
                        //Se actualiza el almacen
                        await _UnitWork.BrandRepository.Update(brandModel);
                        TempData[DS.Success] = "Marca Actualizada correctamente";

                    }

                    await _UnitWork.Save();
                    return RedirectToAction("Index");
                }
                TempData[DS.Error] = "Error al intentar registrar o actualizar Marca";

                return View(brandModel);
            }

            TempData[DS.Error] = "El nombre de Marca que intenta utilizar ya fue asignado a otro";

            return View(brandModel);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int? id)
        {
            var brandToDelete = await _UnitWork.BrandRepository.GetById(id.GetValueOrDefault());
            if (brandToDelete == null)
            {
                return Json(new { success = false, message = "Error al Eliminar Marca" });
            }
            _UnitWork.BrandRepository.Remove(brandToDelete);
            await _UnitWork.Save();
            return Json(new { success = true, message = "Marca Eliminada con Exito" });
        }


        private async Task<bool> CheckName(BrandModel brandModel)
        {
            var listNames = await _UnitWork.BrandRepository.GetAll();
            if (brandModel.IdBrand == 0)
            {
                return listNames.Any(b => b.Name?.ToLower().Trim() == brandModel.Name?.ToLower().Trim());
            }
            else
            {
                return listNames.Any(b => b.Name?.ToLower().Trim() == brandModel.Name?.ToLower().Trim() && b.IdBrand != brandModel.IdBrand);
            }
        }

        #region API

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var allBrands = await _UnitWork.BrandRepository.GetAll();
            return Json(new { data = allBrands });
        }

        #endregion
    }
}
