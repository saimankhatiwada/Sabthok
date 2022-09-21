using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SabthokFoodModel.Utillity;
using SabthokFoodWeb.DataAccess.Repository.IRepository;
using SabthokFoodWeb.Models;
using SabthokFoodWeb.Models.ViewModel;
using System.Security.Claims;

namespace SabthokFoodWeb.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
		public ShoppingCartVM ShoppingCartVM { get; set; }

		public int OrderTotal { get; set; }

		public CartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			ShoppingCartVM = new ShoppingCartVM()
			{
				ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value, includeproperties: "Product"),
				OrderHeader = new()
			};

			foreach(var cart in ShoppingCartVM.ListCart)
			{
				cart.Price = GetPriceBasdOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

            return View(ShoppingCartVM);
		}

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value, includeproperties: "Product"),
                OrderHeader = new()
            };

			ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUsers.GetFirstOrDefault(u => u.Id == claims.Value);
			ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasdOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value, includeproperties: "Product");
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claims.Value;

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasdOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                OrderDetails OrderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetails.Add(OrderDetail);
                _unitOfWork.Save();
            }

            _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
            _unitOfWork.Save();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Plus(int cartID)
		{
			var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartID);
			_unitOfWork.ShoppingCart.IncrementCount(cart, 1);
			_unitOfWork.Save();
			return RedirectToAction("Index");
		}

        public IActionResult Minus(int cartID)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartID);
			if(cart.Count <=1)
			{
                _unitOfWork.ShoppingCart.Remove(cart);
            }
			else
			{
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int cartID)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartID);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        private double GetPriceBasdOnQuantity(double quantity, double price, double price50, double price100)
		{
			if(quantity <=50)
			{
				return price;
			}
			else
			{
				if(quantity <=100)
				{
					return price50;
				}
				return price100;
			}
		}
	}
}
