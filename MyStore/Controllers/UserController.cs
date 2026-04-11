using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyStore.Services;
using System.Security.Claims;

namespace MyStore.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public readonly OrderService _orderService;
        public UserController(OrderService orderService) { 
            _orderService = orderService;
        }
        public async Task<IActionResult> MyOrders()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var odersVM =  await _orderService.GetAllWithDetailsAsync(int.Parse(userId));
            return View(odersVM);
        }
    }
}
