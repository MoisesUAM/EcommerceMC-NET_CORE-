using Ecommerce.BLL.Notifications;
using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.Models.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly IUnitWork _UnitWork;

        public CategoryController(IUnitWork unitWork)
        {
            _UnitWork = unitWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            CategoryModel categoryModel = new CategoryModel();

            if (id == null)
            {
                //Aca se trata de una insercion de nuevo almacen
                categoryModel.Estate = true;
                return View(categoryModel);
            }

            //Aca se trata de una actualizacion de almacen existente
            categoryModel = await _UnitWork.CategoryRepository.GetById(id.GetValueOrDefault());
            if (categoryModel == null)
            {
                return NotFound();
            }

            return View(categoryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CategoryModel category)
        {
            if (!await CheckName(category))
            {
                if (ModelState.IsValid)
                {
                    if (category.IdCategory == 0)
                    {
                        //Se crea un nuevo almacen
                        await _UnitWork.CategoryRepository.Add(category);
                        TempData[DS.Success] = "Categoria creada correctamente";
                    }
                    else
                    {
                        //Se actualiza el almacen
                        await _UnitWork.CategoryRepository.Update(category);
                        TempData[DS.Success] = "Categoria Actualizada correctamente";

                    }

                    await _UnitWork.Save();
                    return RedirectToAction("Index");
                }
                TempData[DS.Error] = "Error al intentar registrar o actualizar categoria";

                return View(category);
            }

            TempData[DS.Error] = "El nombre de Categoria que intenta utilizar ya fue asignado a otro";

            return View(category);
        } 
        
        private async Task<bool> CheckName(CategoryModel category)
        {
            var listNames = await _UnitWork.CategoryRepository.GetAll();
            if (category.IdCategory == 0)
            {
                return listNames.Any(c => c.Name?.ToLower().Trim() == category.Name?.ToLower().Trim());
            }
            else
            {
                return listNames.Any(c => c.Name?.ToLower().Trim() == category.Name?.ToLower().Trim() && c.IdCategory != category.IdCategory);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var categoryToDelete = await _UnitWork.CategoryRepository.GetFirst(c=>c.IdCategory == id);
            if (categoryToDelete == null)
            {
                return Json(new { success = false, message = "Error al Eliminar Almacen" });
            }
            _UnitWork.CategoryRepository.Remove(categoryToDelete);
            await _UnitWork.Save();
            return Json(new { success = true, message = "Categoria Eliminada con Exito" });
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allCategories = await _UnitWork.CategoryRepository.GetAll();
            return Json(new { data = allCategories });
        }

        #endregion
    }
}
