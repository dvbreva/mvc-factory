using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FF.Data;
using FF.Services.Contracts;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using FF.Web.Models.CategoryViewModels;
using System.Threading.Tasks;

namespace FF.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
            ICategoryService categoryService, 
            ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategories();

            return View(categories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var categoryExists = await _categoryService.Exists(id);

            if (!categoryExists)
            {
                return NotFound();
            }

            var category = await _categoryService.GetById(id);

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.Insert(category);

                if (result.Succeeded)
                {
                    TempData["CategoryCreated"] = true;
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            _logger.LogInformation("Category {@category} was inserted.", category);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            ViewData["Products"] = new SelectList(await _categoryService.GetCategories(), "Id", "Name");

            return View(new Category { Name = category.Name, Products = category.Products });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                var categoryExists = await _categoryService.Exists(id);

                if (!categoryExists)
                {
                    return NotFound();
                }

                ViewData["Products"] = new SelectList(await _categoryService.GetCategories(), "Id", "Name", category.Products);

                var result = await _categoryService.Update(id, category);

                if (result.Succeeded)
                {
                    TempData["CategoryEdited"] = true;
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }

            Log.Information("Category {@category} was updated.", category);

            return RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var categoryExists = await _categoryService.Exists(id);

            if (!categoryExists)
            {
                return NotFound();
            }

            var category = await _categoryService.GetById(id);

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _categoryService.Delete(id);

            if (result.Succeeded)
            {
                TempData["CategoryEdited"] = true;
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            Log.Information("Category with id {@id} was deleted.", id);

            return RedirectToAction(nameof(Index));
        }
    }
}
