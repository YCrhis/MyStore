using Microsoft.AspNetCore.Mvc;
using MyStore.Models;
using MyStore.Services;
using MyStore.Utilities;
using System.Diagnostics;
using System.Security.Claims;

namespace MyStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;
        public HomeController(ProductService productService, CategoryService categoryService, OrderService orderService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _orderService = orderService;
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

        public async Task<IActionResult> FilterByCategory(int id, string name)
        {
            var categories = await _categoryService.GetAllAsync();
            var products = await _productService.GetCatalogAsync(categoryId: id);
            var catalogVM = new CatalogVM { Categories = categories, Products = products, FilterBy = name };
            return View("Index", catalogVM);
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

        [HttpPost]
        public async Task<IActionResult> AddItemToCart(int ProductId, int quantity)
        {
            var product = await _productService.GetByIdAsync(ProductId);
            var cart = HttpContext.Session.SessionGet<List<CartItemVM>>("Cart") ?? [];

            if (cart.Find(x => x.ProductId == ProductId) == null)
            {
                cart.Add(new CartItemVM { ProductId = ProductId, Name = product.Name, Price = product.Price, Quantity = quantity, ImageName = product.ImageName! });

            }
            else
            {
                cart.Find(x => x.ProductId == ProductId)!.Quantity += quantity;
            }
            HttpContext.Session.SessionSet("Cart", cart);
            ViewBag.message = "Product added to cart successfully";
            return View("ProductDetail", product);
        }

        public IActionResult RemoveItemCart(int id)
        {
            var cart = HttpContext.Session.SessionGet<List<CartItemVM>>("Cart") ?? [];
            var product = cart.Find(x => x.ProductId == id);
            cart.Remove(product!);
            HttpContext.Session.SessionSet("Cart", cart);
            return View("ViewCart", cart);
        }


        [HttpPost]
        public async Task<IActionResult> PayNow()
        {
            var cart = HttpContext.Session.SessionGet<List<CartItemVM>>("Cart") ?? [];

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            await _orderService.AddAsync(cart, int.Parse(userId));

            HttpContext.Session.Remove("Cart");
            return View("SaleComplete");
        }

        public IActionResult ViewCart()
        {
            var cart = HttpContext.Session.SessionGet<List<CartItemVM>>("Cart") ?? [];
            return View(cart);
        }

        public IActionResult SaleComplete()
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
