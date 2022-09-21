using SabthokFoodWeb.DataAccess.Repository.IRepository;
using SabthokFoodWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodWeb.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }
        public void Update(OrderHeader obj)
        {
            _context.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _context.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if(paymentStatus != null)
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }
    }
}
