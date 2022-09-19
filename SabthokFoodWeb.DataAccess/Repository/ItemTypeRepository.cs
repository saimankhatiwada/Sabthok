using SabthokFoodWeb.DataAccess.Repository.IRepository;
using SabthokFoodWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodWeb.DataAccess.Repository
{
    public class ItemTypeRepository : Repository<ItemType>, IItemTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public ItemTypeRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }
        public void Update(ItemType obj)
        {
            _context.CoverTypes.Update(obj);
        }
    }
}
