using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SabthokFoodModel.Utillity;
using SabthokFoodWeb.DataAccess.Repository.IRepository;
using SabthokFoodWeb.Models;
using SabthokFoodWeb.Models.ViewModel;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using System.Security.Claims;

namespace SabthokFoodWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {

            OrderVM = new OrderVM()
            {
                orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeproperties: "ApplicationUser"),
                orderDetails = _unitOfWork.OrderDetails.GetAll(u => u.OrderId == orderId, includeproperties: "Products"),
            };


            return View(OrderVM);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetail()
        {
            var orderheaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.orderHeader.Id, tracked: false);
            orderheaderFromDb.Name = OrderVM.orderHeader.Name;
            orderheaderFromDb.PhoneNumber = OrderVM.orderHeader.PhoneNumber;
            orderheaderFromDb.StreetAddress = OrderVM.orderHeader.StreetAddress;
            orderheaderFromDb.City = OrderVM.orderHeader.City;
            orderheaderFromDb.State = OrderVM.orderHeader.State;
            orderheaderFromDb.PostalCode = OrderVM.orderHeader.PostalCode;

            if(OrderVM.orderHeader.Carrier != null)
            {
                orderheaderFromDb.Carrier = OrderVM.orderHeader.Carrier;
            }

            if (OrderVM.orderHeader.TrackingNumber != null)
            {
                orderheaderFromDb.TrackingNumber = OrderVM.orderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeader.Update(orderheaderFromDb);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully";
            return RedirectToAction("Details", "Order", new { orderId = orderheaderFromDb.Id });
        }


        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.orderHeader.Id, SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["Success"] = "Order Status Updated Successfully";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.orderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult ShipOrder()
        {
            var orderheaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.orderHeader.Id, tracked: false);
            orderheaderFromDb.TrackingNumber = OrderVM.orderHeader.TrackingNumber;
            orderheaderFromDb.Carrier = OrderVM.orderHeader.Carrier;
            orderheaderFromDb.OrderStatus = SD.StatusShipped;
            orderheaderFromDb.ShippingDate = DateTime.Now;
            if(orderheaderFromDb.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderheaderFromDb.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            _unitOfWork.OrderHeader.Update(orderheaderFromDb);
            _unitOfWork.Save();
            TempData["Success"] = "Order Shipped Successfully";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.orderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            var orderheaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.orderHeader.Id, tracked: false);
            if(orderheaderFromDb.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderheaderFromDb.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStatus(orderheaderFromDb.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderheaderFromDb.Id, SD.StatusCancelled, SD.StatusCancelled);
            }
            _unitOfWork.Save();
            TempData["Success"] = "Order Cancelled Successfully";
            return RedirectToAction("Details", "Order", new { orderId = OrderVM.orderHeader.Id });
        }



        [ActionName("Details")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details_PAY_NOW()
        {
            OrderVM.orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.orderHeader.Id, includeproperties: "ApplicationUser");
            OrderVM.orderDetails = _unitOfWork.OrderDetails.GetAll(u => u.OrderId == OrderVM.orderHeader.Id, includeproperties: "Products");

            //stripe settings 
            var domain = "https://sabthokfood.azurewebsites.net/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderid={OrderVM.orderHeader.Id}",
                CancelUrl = domain + $"admin/order/Details?orderId={OrderVM.orderHeader.Id}",
            };

            foreach (var item in OrderVM.orderDetails)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)((item.Price / 127) * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Products.Title
                        },

                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentId(OrderVM.orderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }


        public IActionResult PaymentConfirmation(int orderHeaderid)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderHeaderid);
            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentId(orderHeaderid, orderHeader.SessionId, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeaderid, orderHeader.OrderStatus, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }

            }
            return View(orderHeaderid);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;

            if(User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(includeproperties: "ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == claims.Value, includeproperties: "ApplicationUser");
            }

            

            switch(status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }

            return Json(new { data = orderHeaders });
        }

        #endregion
    }


}
