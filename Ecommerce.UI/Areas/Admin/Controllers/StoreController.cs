using Ecommerce.BLL.Notifications;
using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.Models.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.AdminRole)]
    public class StoreController : Controller
    {
        private readonly IUnitWork _unitWork;

        public StoreController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            StoreModel store = new StoreModel();

            if (id == null)
            {
                //Aca se trata de una insercion de nuevo almacen
                store.Estate = true;
                return View(store);
            }

            //Aca se trata de una actualizacion de almacen existente
            store = await _unitWork.StoreRepository.GetById(id.GetValueOrDefault());
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(StoreModel store)
        {
            if (!await ChekcName(store))
            {
                if (ModelState.IsValid)
                {
                    if (store.IdStore == 0)
                    {
                        //Se crea un nuevo almacen
                        await _unitWork.StoreRepository.Add(store);
                        TempData[DS.Success] = "Almacen creado correctamente";
                    }
                    else
                    {
                        //Se actualiza el almacen
                        await _unitWork.StoreRepository.Update(store);
                        TempData[DS.Success] = "Almacen Actualizado correctamente";

                    }

                    await _unitWork.Save();
                    return RedirectToAction("Index");
                }
                TempData[DS.Error] = "Error al intentar registrar o actualizar almacen";

                return View(store);
            }
      
                TempData[DS.Error] = "El nombre de Almacen que intenta utilizar ya fue asignado a otro";

                return View(store);

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var storeToDelete = await _unitWork.StoreRepository.GetFirst(s=>s.IdStore == id);
            if(storeToDelete == null)
            {
                return Json(new {success = false, message = "Error al Eliminar Almacen"});
            }
            _unitWork.StoreRepository.Remove(storeToDelete);
            await _unitWork.Save();
            return Json(new { success = true, message = "Bodega Eliminada con Exito" });
        }

        private async Task<bool> ChekcName(StoreModel store)
        {
            var listNames = await _unitWork.StoreRepository.GetAll();
            if(store.IdStore == 0)
            {
                return listNames.Any(s => s.Name?.ToLower().Trim() == store.Name?.ToLower().Trim());
            }
            else
            {
                return listNames.Any(s => s.Name?.ToLower().Trim() == store.Name?.ToLower().Trim() && s.IdStore != store.IdStore);
            }
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allStores = await _unitWork.StoreRepository.GetAll();
            return Json(new { data = allStores });
        }

        #endregion

    }
}
