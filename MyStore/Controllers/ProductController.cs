using Microsoft.AspNetCore.Mvc;
using MyStore.Models;
using MyStore.Services;

namespace MyStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            var productVM = await _productService.GetByIdAsync(id);
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductVM entity)
        {
            ViewBag.message = null;
            ModelState.Remove("Categories");
            ModelState.Remove("Category.Name");
            if (!ModelState.IsValid) return View(entity);

            if (entity.ProductId == 0)
            {
                await _productService.AddAsync(entity);
                ModelState.Clear();
                entity = new ProductVM();
                ViewBag.message = "The product was created";
            }
            else
            {
                await _productService.EditAsync(entity);
                ViewBag.message = "The product was updated";
            }

            return View(entity);

        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _productService.GetByIdAsync(id);
            await _productService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
