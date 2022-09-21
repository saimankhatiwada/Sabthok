using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SabthokFoodWeb.DataAccess.Repository.IRepository;
using SabthokFoodWeb.Models;
using SabthokFoodWeb.Models.ViewModel;
using System.Diagnostics;
using System.Security.Claims;

namespace SabthokFoodWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Products> productlist = _unitOfWork.Product.GetAll(includeproperties: "Category,CoverType");
            return View(productlist);
        }

        public IActionResult Details(int ProductId)
        {
            ShoppingCart shopingcart = new()
            {
                Count = 1,
                ProductId = ProductId,
                Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == ProductId, includeproperties: "Category,CoverType")
            };

            return View(shopingcart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claims.Value;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                u=>u.ApplicationUserId == claims.Value && u.ProductId == shoppingCart.ProductId);

            if(cartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            else
            {
                _unitOfWork.ShoppingCart.IncrementCount(cartFromDb,shoppingCart.Count);
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
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