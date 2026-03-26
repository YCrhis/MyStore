using Microsoft.AspNetCore.Mvc;
using MyStore.Models;
using MyStore.Services;
using System.Diagnostics;

namespace MyStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;
        public HomeController(ProductService productService, CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            var products = await _productService.GetCatalogAsync();
            var catalogVM = new CatalogVM()
            {
                Categories = categories,
                Products = products
            };
            return View(catalogVM);
        }

        public async Task<IActionResult> FilterByCategory(int id , string name)
        {
            var categories = await _categoryService.GetAllAsync();
            var products = await _productService.GetCatalogAsync(categoryId:id);
            var catalogVM = new CatalogVM {Categories = categories, Products = products, FilterBy=name };
            return View("Index",catalogVM);
        }
        [HttpPost]
        public async Task<IActionResult> FilterBySearch(string value)
        {
            var categories = await _categoryService.GetAllAsync();
            var products = await _productService.GetCatalogAsync(search: value);
            var catalogVM = new CatalogVM { Categories = categories, Products = products, FilterBy = $"Results for: {value}" };
            return View("Index", catalogVM);
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return View(product);
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
