using Ecommerce.BLL.Notifications;
using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.Models.Catalog;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitWork _UnitWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public ProductController(IUnitWork unitWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitWork = unitWork ?? throw new ArgumentNullException(nameof(unitWork));
            _WebHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Upsert(int? id)
        {

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new ProductModel(),
                BrandSelectItemList = await _UnitWork.ProductRepository.GetListBrandRecords(),
                CategorySelectItemList = await _UnitWork.ProductRepository.GetListCategoryRecords(),
                ProductSelectItemList = await _UnitWork.ProductRepository.GetListProductRecords()
            };

            if (id == null)
            {
                //Aca se trata de una insercion de nuevo producto
                productViewModel.Product.Estate = true;
                return View(productViewModel);
            }

            //Aca se trata de una actualizacion de almacen existente
            productViewModel.Product = await _UnitWork.ProductRepository.GetById(id.GetValueOrDefault());
            if (productViewModel.Product == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upsert(ProductViewModel productViewModel)
        {
            //Se vuelven a cargar las listas desplegables en caso de necesitar volver a la vista
            productViewModel.CategorySelectItemList = await _UnitWork.ProductRepository.GetListCategoryRecords();
            productViewModel.BrandSelectItemList = await _UnitWork.ProductRepository.GetListBrandRecords();
            productViewModel.ProductSelectItemList = await _UnitWork.ProductRepository.GetListProductRecords();
#pragma warning disable CS8604 // Possible null reference argument.
            if (!await CheckSerialNumber(productViewModel.Product))
            {
                if (ModelState.IsValid)
                {
                    var files = HttpContext.Request.Form.Files;
                    string webRootPath = _WebHostEnvironment.WebRootPath;
                    if (productViewModel.Product!.IdProduct == 0)
                    {
                        //este caso es para un nuevo producto
                        string upload = webRootPath + DS.ImagesRootPaht;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productViewModel.Product.ImageUrl = fileName + extension;
                        await _UnitWork.ProductRepository.Add(productViewModel.Product);
                        TempData[DS.Success] = "Producto creado correctamente";
                        await _UnitWork.Save();
                        return View("Index");
                    }
                    else
                    {
                        //esta caso es para actualizar
                        var productToUpdate = await _UnitWork.ProductRepository.GetFirst(p => p.IdProduct == productViewModel.Product.IdProduct, isTracking: false);
                        //se verifica si el usuario va a actualzar la imagen
                        if (files.Count > 0)
                        {
                            string upload = webRootPath + DS.ImagesRootPaht;
                            string fileName = Guid.NewGuid().ToString();
                            string extension = Path.GetExtension(files[0].FileName);

                            //Se debe borrar del directorio raiz la imagen anterior
#pragma warning disable CS8604 // Possible null reference argument.
                            var oldFile = Path.Combine(upload, productToUpdate.ImageUrl);
#pragma warning restore CS8604 // Possible null reference argument.
                            if (System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }
                            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                            {
                                files[0].CopyTo(fileStream);
                            }
                            // se actualiza el path de la imagen nueva
                            productViewModel.Product.ImageUrl = fileName + extension;
                        }
                        else
                        {
                            productViewModel.Product.ImageUrl = productToUpdate.ImageUrl;
                        }

                        await _UnitWork.ProductRepository.Update(productViewModel.Product);
                        TempData[DS.Success] = "Producto actualizado correctamente";
                        await _UnitWork.Save();
                        return View("Index");
                    }
                }
            }
            else
            {
                if (productViewModel.Product.IdProduct == 0)
                {
                    TempData[DS.Error] = "El Numero de Serie que esta intentando asignar ya esta siendo utilizado";
                    return View(productViewModel);
                }
                else
                {
                    productViewModel.Product = await _UnitWork.ProductRepository.GetFirst(p => p.IdProduct == productViewModel.Product.IdProduct, isTracking: false);
                    TempData[DS.Error] = "El Numero de Serie que esta intentando asignar ya esta siendo utilizado";
                    return View(productViewModel);
                }
            }

            TempData[DS.Error] = "Error al cargar los datos";
            return View(productViewModel);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int? id)
        {
            var productToDelete = await _UnitWork.ProductRepository.GetById(id.GetValueOrDefault());
            if (productToDelete == null)
            {
                return Json(new { success = false, message = "Error al Eliminar Producto" });
            }

            //Borrar imagen de almacenamiento local
            string upload = _WebHostEnvironment.WebRootPath + DS.ImagesRootPaht;
            var fileToDelete = Path.Combine(upload, productToDelete.ImageUrl);
            if (System.IO.File.Exists(fileToDelete))
            {
                System.IO.File.Delete(fileToDelete);
            }
            _UnitWork.ProductRepository.Remove(productToDelete);
            await _UnitWork.Save();
            return Json(new { success = true, message = "Producto Eliminado con Exito" });
        }


        private async Task<bool> CheckSerialNumber(ProductModel product)
        {
            var listNames = await _UnitWork.ProductRepository.GetAll();
            if (product.IdProduct == 0)
            {
                return listNames.Any(p => p.SerialNumber?.ToLower().Trim() == product.SerialNumber?.ToLower().Trim());
            }
            else
            {
                return listNames.Any(p => p.SerialNumber?.ToLower().Trim() == product.SerialNumber?.ToLower().Trim() && p.IdProduct != product.IdProduct);
            }
        }

        #region API

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var allProducts = await _UnitWork.ProductRepository.GetAll(includedProperties:"Category,Brand");
            return Json(new { data = allProducts });
        }

        #endregion
    }
}
