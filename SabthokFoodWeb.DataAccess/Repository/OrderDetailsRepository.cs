using SabthokFoodWeb.DataAccess.Repository.IRepository;
using SabthokFoodWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodWeb.DataAccess.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailsRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }
        public void Update(OrderDetails obj)
        {
            _context.OrderDetails.Update(obj);
        }
    }
}
