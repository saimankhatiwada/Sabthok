using Microsoft.AspNetCore.Mvc;
using SabthokFoodWeb.DataAccess.Repository.IRepository;
using SabthokFoodWeb.Models;
using SabthokFoodWeb.Models.ViewModel;
using System.Diagnostics;

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

        public IActionResult Details(int? id)
        {
            ShoppingCart shopingcart = new()
            {
                Count = 1,
                product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id, includeproperties: "Category,CoverType")
            };

            return View(shopingcart);
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