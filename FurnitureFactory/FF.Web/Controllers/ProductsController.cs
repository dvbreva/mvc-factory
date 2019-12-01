using System.Threading.Tasks;
using FF.Data.Entities;
using FF.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FF.Web.Controllers
{
    [Authorize(Roles = "Admin,Regular")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();

            return View(products);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var productExists = await _productService.Exists(id);

            if (!productExists)
            {
                return NotFound();
            }

            var product = await _productService.GetById(id);

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategories(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategories(), "Id", "Name", product.CategoryId);

                var result = await _productService.Insert(product);

                if (result.Succeeded)
                {
                    TempData["ProductAdded"] = true;
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            return RedirectToAction("Index","Products");
        }

        // edit
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategories(), "Id", "Name");

            return View(new Product
            {
                Name = product.Name,
                Description = product.Description,
                WeightInKilos = product.WeightInKilos,
                Price = product.Price,
                CategoryId = product.CategoryId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (ModelState.IsValid)
            {
                var productExists = await _productService.Exists(id);

                if (!productExists)
                {
                    return NotFound();
                }

                ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategories(), "Id", "Name", product.CategoryId);

                var result = await _productService.Update(id, product);
                if (result.Succeeded)
                {
                    TempData["OrderDeleted"] = true;
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            return RedirectToAction("Details", new { id });
        }

        // delete
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var productExists = await _productService.Exists(id);

            if (!productExists)
            {
                return NotFound();
            }

            var product = await _productService.GetById(id);

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.Delete(id);

                if (result.Succeeded)
                {
                    TempData["OrderDeleted"] = true;
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
           
            return RedirectToAction("Index");
        }
    }
}
